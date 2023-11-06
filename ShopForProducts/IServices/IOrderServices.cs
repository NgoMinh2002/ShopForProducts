using Microsoft.AspNetCore.Mvc;
using ShopForProducts.Entities;
using ShopForProducts.Model;

namespace ShopForProducts.IServices
{
    public interface IOrderServices
    {   
        Task<Order> Createorder(int UrseId, OrderModel orderModel);
        Task<Order> UpdateOrder(int OrderId,int UserId, OrderModel orderModel);
        Task<Order> UpdatePrices(int orderDetailId);
        Task<Order> DeleteOrder(int OrderId);
        Task<List<OrderWithStatusModel>> GetOrder_Statuses();
        Task<List<Order>> GetOrdersByStatus(string status);
        Task<OrderDetailsDTO> GetOrderDetails(int orderId);
        Task<Order> UpdateOrderStatusAndSendEmail(int orderId,int status);
    }
}
