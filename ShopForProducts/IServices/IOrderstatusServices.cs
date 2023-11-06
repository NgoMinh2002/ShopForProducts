using ShopForProducts.Entities;

namespace ShopForProducts.IServices
{
    public interface IOrderstatusServices
    {
        Task<Order_status> AddOrer_Status(string nameStatus);
        Task<Order_status> AupdateOrder_Status(Order_status status);
        Task<Order_status> DeleteOrder_status(string nameStatus);
    }
}
