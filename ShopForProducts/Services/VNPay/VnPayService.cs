using ShopForProducts.Entities;
using ShopForProducts.Libraries;
using ShopForProducts.Model.VNPay;
using ShopForProducts.IServices.VNPay;
using Microsoft.EntityFrameworkCore;
using MailKit.Search;

namespace ShopForProducts.Services.VNPay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbcontext appDbcontext;
        public VnPayService(IConfiguration configuration)
        {
            appDbcontext = new AppDbcontext();
            _configuration = configuration;
        }

        public string CreatePaymen(int OrderId, int paymentId, HttpContext context)
        {
           /* var vnpay = new VnPayLibrary();*/
            if (appDbcontext.dboOrders.Any(x=>x.OrderId == OrderId)) {
                var orrder = appDbcontext.dboOrders.FirstOrDefault(x=>x.OrderId==OrderId);
                var paymentUrt = string.Empty;
                switch (paymentId)
                {
                    case 1:
                        orrder.Order_statusId = 1;
                        orrder.updated_at = DateTime.Now;
                        orrder.PaymentId = paymentId;
                        UpdateStockViewProduct(OrderId);
                        break;
                    case 2:
                         var vnpay = new VnPayLibrary();

                        paymentUrt =   CreatePaymentUrl(OrderId, context);
                        // Cập nhật trạng thái đơn hàng và hàng tồn kho
                        orrder.Order_statusId = 2;
                        orrder.PaymentId = paymentId;
                        orrder.updated_at = DateTime.Now;
                        UpdateStockViewProduct(OrderId);
                        break;

                }
                return paymentUrt;
            }
            else
            
                throw new Exception($"Order id {OrderId} khong ton tai! Kiem tra lai!");
           


        }

        public string CreatePaymentUrl(int orderId, HttpContext context)
        {
            var order = appDbcontext.dboOrders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại"); // Trả về 404 Not Found nếu không tìm thấy Order
            }
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)order.actual_price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{order.full_name} thanh toán số tiền   {order.actual_price} cho đơn hàng {order.OrderId}");
            pay.AddRequestData("vnp_OrderType", "normal");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
               pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, hashSecret: _configuration["Vnpay:HashSecret"]);
            if (response.Success)
            {
                var Vnpay = new VnPay()
                {
                    OrderId = response.OrderId,
                    OrderDescription = response.OrderDescription,
                    TransactionId = response.TransactionId,
                    PaymentId = response.PaymentId,
                    PaymentMethod = response.PaymentMethod,
                    Token = response.Token,
                    VnPayResponseCode = response.VnPayResponseCode,
                };
                appDbcontext.vnPays.Add(Vnpay);
                appDbcontext.SaveChanges();
                
            }
            return response;
        }
        private void UpdateStockViewProduct(int orderID)
        {
            var orderHT = appDbcontext.dboOrders.FirstOrDefault(x => x.OrderId == orderID);
            var dsOrder_DetailHT = appDbcontext.dboOrder_detail.Where(x => x.OrderId == orderID).ToList();
            foreach (var detail in dsOrder_DetailHT)
            {
                var productHT = appDbcontext.dboProducts.FirstOrDefault(x => x.ProductId == detail.ProductId);
                productHT.updated_at = DateTime.Now;
                appDbcontext.Update(productHT);
                appDbcontext.SaveChanges();
            }
        }
    }
}
