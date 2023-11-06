using ShopForProducts.Entities;
using ShopForProducts.Model;

namespace ShopForProducts.IServices
{
    public interface ICartServices
    {
        Task<Carts> CreateCarts(int userId, int productId, int quantity);
        Task<Carts> DeteleCarts(int CartsId);
        Task<Order> CreateOrderFromCart(int cartId/*, List<Cart_item> cartItems*/, OrderUpdateModel orderModel);
        Task<List<Cart_item>> GetCarts(int UserId);
    }
}
