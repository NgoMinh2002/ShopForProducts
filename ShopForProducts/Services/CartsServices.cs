using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopForProducts.Services
{
    public class CartsServices : ICartServices
    {
        private readonly AppDbcontext appDbcontext;
        public CartsServices()
        {
            appDbcontext = new AppDbcontext();
        }
        public async Task<Carts> CreateCarts(int userId, int productId, int quantity)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var userCart = await appDbcontext.dboCrats
                        .Include(c => c.Cart_Items)
                        .Where(cart => cart.UserId == userId)
                        .FirstOrDefaultAsync();
                    if (userCart == null)
                    {
                        var nuserCart = new Carts()
                        {
                            UserId = userId,
                            created_at = DateTime.Now,
                        };
                        var cart_s = new List<Cart_item>();
                        var product = appDbcontext.dboProducts.FirstOrDefaultAsync(c => c.ProductId == productId);
                        if (product == null)
                        {
                            throw new Exception("Sản phẩm không tồn tại");
                        }
                        else
                        {
                            var newCartItem = new Cart_item()
                            {
                                CartId = nuserCart.CartId,
                                ProductId = productId,
                                quantity = quantity,
                                created_at = DateTime.Now,
                            };
                            cart_s.Add(newCartItem);
                            await appDbcontext.SaveChangesAsync();
                        }
                        nuserCart.Cart_Items = cart_s;
                        await appDbcontext.SaveChangesAsync();
                        appDbcontext.dboCart_item.AddRange(cart_s);
                        appDbcontext.dboCrats.Add(nuserCart);
                        await appDbcontext.SaveChangesAsync();
                    }
                    else
                    {
                        var existingCartItem = userCart.Cart_Items.FirstOrDefault(item => item.ProductId == productId);

                        if (existingCartItem != null)
                        {
                            existingCartItem.quantity += quantity;
                            existingCartItem.updated_at = DateTime.Now;
                            await appDbcontext.SaveChangesAsync();
                        }
                        else
                        {
                            var newCartItem = new Cart_item()
                            {
                                CartId = userCart.CartId,
                                ProductId = productId,
                                quantity = quantity,
                                created_at = DateTime.Now,
                            };
                            appDbcontext.dboCart_item.Add(newCartItem);
                            await appDbcontext.SaveChangesAsync();
                        }
                    }
                    transaction.Commit();
                    return userCart;
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }
        }
        public async Task<Order> CreateOrderFromCart(int cartId /*List<Cart_item> cartItems*/, OrderUpdateModel orderModel)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var cart = await appDbcontext.dboCrats.Include(c => c.Cart_Items).FirstOrDefaultAsync(c => c.CartId == cartId);
                    if (cart == null)
                    {
                        throw new Exception("Không tìm thấy giỏ hàng");
                    }
                    var user = await appDbcontext.dboAccounts.FirstOrDefaultAsync(u => u.UserId == cart.UserId);
                    if (user == null)
                    {
                        throw new Exception("không tìm thấy người dùng");
                    }
                    var order = new Order()
                    {
                        UserId = cart.UserId,
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
                    /*appDbcontext.dboOrders.Add(order);
                    appDbcontext.SaveChanges();*/
                    double? totalOrderPrice = 0.0;
                    var orderDetails = new List<Order_detail>();
                    foreach (var cartItem in cart.Cart_Items)
                    {
                        var product = appDbcontext.dboProducts.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                        if (product == null)
                        {
                            continue;
                        }
                        var orderDetail = new Order_detail()
                        {
                            OrderId = order.OrderId,
                            ProductId = cartItem.ProductId,
                            quantity = cartItem.quantity,
                            price_total = product.price * cartItem.quantity * (100 - product.discount) / 100,
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,
                        };
                        totalOrderPrice += orderDetail.price_total;
                        orderDetails.Add(orderDetail);
                       await appDbcontext.SaveChangesAsync();
                    }
                    order.order_detail = orderDetails;
                    order.original_price = totalOrderPrice;
                    order.actual_price = order.original_price * 0.9;
                    appDbcontext.dboOrder_detail.AddRange(orderDetails);
                    appDbcontext.dboOrders.Add(order);
                    await appDbcontext.SaveChangesAsync();
                    /*appDbcontext.dboCrats.Remove(cart);
                    await appDbcontext.SaveChangesAsync();*/
                    var jsonOptions = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    };
                    var orderJson = JsonSerializer.Serialize(order, jsonOptions);
                    transaction.Commit();
                    return order;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }
        public async Task<Carts> DeteleCarts(int CartsId)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var carts = await appDbcontext.dboCrats.Include(o => o.Cart_Items).FirstOrDefaultAsync(o => o.CartId == CartsId);
                    if (carts == null)
                    {
                        throw new Exception("Không tìm thấy giỏ hàng để xóa");
                    }
                    else
                    {
                        appDbcontext.dboCart_item.RemoveRange(carts.Cart_Items);
                        appDbcontext.dboCrats.Remove(carts);
                        await appDbcontext.SaveChangesAsync();
                        transaction.Commit();
                        return carts;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi:" + ex.Message);
                }
            }
        }
        public async Task<List<Cart_item>> GetCarts(int UserId)
        {
            var cartItem = await appDbcontext.dboCrats.Include(c => c.Cart_Items).Where(c => c.UserId == UserId).
                SelectMany(c => c.Cart_Items).ToListAsync();
            return cartItem;
        }
    }
}
