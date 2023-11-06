using Microsoft.EntityFrameworkCore;
using MimeKit;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using MailKit.Net.Smtp;
using ShopForProducts.IServices.VNPay;

namespace ShopForProducts.Services
{
    public class OrderServices : IOrderServices
    {
        private AppDbcontext appDbcontext;
       
        public OrderServices()
        {
            appDbcontext = new AppDbcontext();
        }
        public async Task<bool> IsPaymentValidAsync(int? paymentId)
        {
            // Kiểm tra PaymentId trong cơ sở dữ liệu
            var payment = await appDbcontext.dboPayment.FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            // Kiểm tra trạng thái của phương thức thanh toán
            if (payment != null && payment.status == 1) // 1 là trạng thái hoạt động
            {
                return true;
            }

            return false;
        }
        public async Task<Order> Createorder(int UrseId, OrderModel orderModel)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await appDbcontext.dboAccounts.FirstOrDefaultAsync(u => u.UserId ==UrseId);
                    if (user == null)
                    {
                        throw new Exception("Người dùng không tồn tại");
                    }
                    else
                    {
                        if (!await IsPaymentValidAsync(orderModel.PaymentId))
                        {
                            throw new InvalidOperationException("Phương thức thanh toán không hợp lệ hoặc không hoạt động.");
                        }
                        else
                        {
                            var order = new Order()
                            { 
                                UserId = user.UserId,
                                PaymentId = orderModel.PaymentId,
                                Email = orderModel.Email,
                                phone = orderModel.phone,
                                full_name = orderModel.full_name,
                                address = orderModel.address,
                                actual_price = 0.0,
                                original_price = 0.0,
                                Order_statusId = 1,
                                created_at = DateTime.Now,

                            };
                            /* appDbcontext.dboOrders.Add(order);*/
                            /*  await appDbcontext.SaveChangesAsync();*/

                            double? totalOrderPrice = 0.0;
                            var orderDetails = new List<Order_detail>();
                            ;
                            foreach (var item in orderModel.order_detail)
                            {
                                var product = appDbcontext.dboProducts.Find(item.ProductId);
                                if (product == null)
                                {
                                    throw new Exception("Sản phẩm không tồn tại");
                                }
                                else
                                {
                                    var orderDetail = new Order_detail()
                                    {
                                        OrderId = order.OrderId,
                                        ProductId = item.ProductId,
                                        quantity = item.quantity,
                                        price_total = product.price * item.quantity * (100 - product.discount) / 100,
                                        created_at = DateTime.Now,
                                        updated_at = DateTime.Now,
                                    };
                                    totalOrderPrice += orderDetail.price_total;
                                    orderDetails.Add(orderDetail);
                                    await appDbcontext.SaveChangesAsync();
                                }
                            }
                            order.order_detail = orderDetails;
                            order.original_price = totalOrderPrice;
                            order.actual_price = order.original_price * 0.9;
                            appDbcontext.dboOrder_detail.AddRange(orderDetails);
                            appDbcontext.dboOrders.Add(order);
                            await appDbcontext.SaveChangesAsync();
                         
                            var jsonOptions = new JsonSerializerOptions
                            {
                                ReferenceHandler = ReferenceHandler.Preserve
                            };
                            var orderJson = JsonSerializer.Serialize(order, jsonOptions);
                            transection.Commit();
                            return order;
                        }
                    }

                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<Order> UpdateOrder(int OrderId,int UserId, OrderModel orderModel)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {

                    var order = await appDbcontext.dboOrders
                 .Include(o => o.order_detail)
                 .FirstOrDefaultAsync(o => o.OrderId == OrderId);

                    if (order == null)
                    {
                        throw new Exception("Đơn hàng không tồn tại");
                    }
                    else
                    {
                        if (UserId > 0)
                        {
                            order.UserId = UserId;
                        }
                        if (!string.IsNullOrEmpty(orderModel.Email))
                        {
                            order.Email = orderModel.Email;
                        }
                        if (!string.IsNullOrEmpty(orderModel.full_name))
                        {
                            order.full_name = orderModel.full_name;
                        }
                        if (!string.IsNullOrEmpty(orderModel.phone))
                        {
                            order.phone = orderModel.phone;
                        }
                        if (!string.IsNullOrEmpty(orderModel.address))
                        {
                            order.address = orderModel.address;
                        }
                        if (orderModel.PaymentId > 0)
                        {
                            order.PaymentId = orderModel.PaymentId;
                        }
                        order.updated_at = DateTime.Now;
                        appDbcontext.dboOrders.Update(order);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                        return order;
                    }
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Lỗi không thể cập nhật! " + ex.Message);
                }
            }
        }
        public async Task<Order> UpdatePrices(int orderDetailId)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderDetail = await appDbcontext.dboOrder_detail.FirstOrDefaultAsync(od => od.Order_detailId == orderDetailId);
                    if (orderDetailId == null)
                    {
                        throw new Exception("Đơn hàng chi tiết không có");
                    }
                    else
                    {
                        var order = await appDbcontext.dboOrders.FirstOrDefaultAsync(o => o.OrderId == orderDetail.OrderId);
                        if (order == null)
                        {
                            throw new Exception("Đơn hàng không có");
                        }
                        else
                        {
                            var remainingOrderDetails = await appDbcontext.dboOrder_detail
                        .Where(od => od.OrderId == order.OrderId && od.Order_detailId != orderDetailId)
                        .ToListAsync();
                            double totalOriginalPrice = (double)remainingOrderDetails.Sum(od => od.price_total);
                            double totalActualPrice = totalOriginalPrice * 0.9;

                            // Cập nhật lại giá trị original_price và actual_price cho đơn hàng
                            order.original_price = totalOriginalPrice;
                            order.actual_price = totalActualPrice;

                            // Xóa đơn hàng chi tiết đã bị xóa
                            appDbcontext.dboOrder_detail.Remove(orderDetail);

                            // Lưu các thay đổi vào cơ sở dữ liệu
                            await appDbcontext.SaveChangesAsync();

                            // Commit giao dịch
                            transaction.Commit();
                            return order;

                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Có lỗi xảy ra khi cập nhật giá trị đơn hàng: " + ex.Message);
                }
            }
        }
        public async Task<Order> DeleteOrder(int OrderId)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await appDbcontext.dboOrders.Include(o => o.order_detail).FirstOrDefaultAsync(o => o.OrderId == OrderId);
                    if (order == null)
                    {
                        throw new Exception("Không tìm thấy đơn hàng để xóa");
                    }
                    else
                    {
                        appDbcontext.dboOrder_detail.RemoveRange(order.order_detail);
                        appDbcontext.dboOrders.Remove(order);
                        await appDbcontext.SaveChangesAsync();
                        transaction.Commit();
                        return order;

                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<Order>> GetOrdersByStatus(string status)
        {
            var orders = await appDbcontext.dboOrders
                .Include(order => order.status)
                .Where(order => order.status.status_name == status)
                .ToListAsync();

            return orders;
        }

        public async Task<List<OrderWithStatusModel>> GetOrder_Statuses()

        {
            try
            {
                var ordersWithStatus = await appDbcontext.dboOrders
            .Include(order => order.status) // Nạp trạng thái đơn hàng
            .Select(order => new OrderWithStatusModel()
            {
                OrderId = order.OrderId,
                PaymentId = order.PaymentId,
                full_name = order.full_name,
                phone = order.phone,
                Email = order.Email,
                address = order.address,
                actual_price = order.actual_price,
                original_price = order.original_price,
                status_name = order.status.status_name
            })
            .ToListAsync();
                return ordersWithStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await appDbcontext.dboOrders
                .Include(o => o.status)
                .Include(o => o.order_detail)
                .ThenInclude(od => od.Product)
                .Where(o => o.OrderId == orderId)
                .Select(o => new OrderDetailsDTO
                {
                    OrderId = o.OrderId,
                    FullName = o.full_name,
                    Email = o.Email,
                    Phone = o.phone,
                    Address = o.address,
                    OriginalPrice = (double)o.original_price,
                    ActualPrice = (double)o.actual_price,
                    OrderStatus = o.status.status_name,
                    OrderDetails = o.order_detail.Select(od => new OrderDetailDTO
                    {
                        ProductId = od.Product.ProductId,
                        ProductName = od.Product.name_product,
                        PriceTotal = (double)od.price_total,
                        Quantity = (int)od.quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<Order> UpdateOrderStatusAndSendEmail(int orderId, int status)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await appDbcontext.dboOrders
                     .Include(o => o.status) // Ensure that Status navigation property is loaded
                     .FirstOrDefaultAsync(o => o.OrderId == orderId);
                    if (order == null)
                    {
                        throw new Exception("Lỗi không tìm thấy đơn hàng!");
                    }
                    else
                    {
                        var newStatus = await appDbcontext.dboOrder_status
                            .FirstOrDefaultAsync(s => s.Order_statusId == status);
                        if (newStatus == null)
                        {
                            throw new Exception("Lỗi không có trạng thái này!");
                        }
                        else
                        {
                            string oldStatus = order.status.status_name;
                            order.Order_statusId = status;

                            appDbcontext.dboOrders.Update(order);
                            await appDbcontext.SaveChangesAsync();
                            string userEmailAddress = order.Email;
                            
                            string subject = "Cập nhật trạng thái đơn hàng";
                            string body = $"Trạng thái đơn hàng có mã {orderId} đã được thay đổi từ '{oldStatus}' thành {order.status.status_name}.";
                            SendEmail(userEmailAddress, subject, body);
                            transection.Commit();
                        }
                        return order;
                    }
                   
                }

                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Lỗi: "+ ex.Message );
                }
            }
        }
        public void SendEmail(string userEmailAddress, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com"; // Thay thế bằng máy chủ SMTP của bạn
            int smtpPort = 587; // Port của máy chủ SMTP
            string fromEmail = "anhkunvipzz@gmail.com"; // Địa chỉ email nguồn
            string password = "yvin mhtv yipm gatv\r\n"; // Mật khẩu của địa chỉ email nguồn
          // Nạp thông tin trạng thái


            using (var client = new SmtpClient())
            {
             
                client.Connect(smtpServer, smtpPort, false);
                client.Authenticate(fromEmail, password);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your Name", fromEmail)); // Địa chỉ email nguồn
                message.To.Add(new MailboxAddress("", userEmailAddress)); // Sử dụng địa chỉ email từ biến userEmailAddress
                message.Subject = "Thông báo cập nhật trạng thái";
                message.Body = new TextPart("plain")
                {
                    Text = subject + body

                };

                try
                {
                    client.Send(message);
                    Console.WriteLine("Email xác thực đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi gửi email: " + ex.Message);
                }
                client.Disconnect(true);
            }
        }
      
        
    }
}
