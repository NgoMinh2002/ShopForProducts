
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using ShopForProducts.Services;
using System.Security.Claims;

namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrDerController : ControllerBase
    {
        public readonly IOrderServices orderServices;
        public OrDerController()
        {
            orderServices = new OrderServices();
        }
        [Authorize(Roles = "User")]
        [HttpPost("Buydirectly")]
        public async Task<IActionResult> Createorder( [FromBody] OrderModel orderModel)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
                {
                    await orderServices.Createorder(userIdInt, orderModel);
                    return Ok("Đã tạo đơn hàng thành công.");
                }
                else
                {
                    return Unauthorized("Vui lòng đăng nhập mới được đặt hàng.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [Authorize(Roles = "User")]
        [HttpPut("UpdateOder/{OrderId}")]
        public async Task<IActionResult> Updateorder(int OrderId, [FromBody] OrderModel orderModel)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
                {
                    var update = await orderServices.UpdateOrder(userIdInt,OrderId, orderModel);

                    return Ok(new { message = "Thêm thành công đơn hàng.", oder = update });
                }
                else
                {
                    return BadRequest("Cần đăng nhập");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("orderDetail/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderDetailId)
        {
            try
            {

                await orderServices.UpdatePrices(orderDetailId);

                return Ok("Đã xóa đơn hàng chi tiết và cập nhật đơn hàng thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("DeteleOder/{OrderId}")]
        public async Task<IActionResult> DeleteOrder(int OrderId)
        {
            try
            {
                await orderServices.DeleteOrder(OrderId); return Ok("Đã xóa đơn hàng");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("ShowStatus")]
        public async Task<IActionResult> GetOrdersStatus(string status)
        {
            try
            {
                var ordersWithStatus = await orderServices.GetOrdersByStatus(status);
                return Ok(ordersWithStatus);
            }
            catch(Exception ex)
            {
                return BadRequest("danh sách trống"+ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("ShowOrders")]
        public async Task<IActionResult> GetOrders(int OrderId)
        {
            try
            {
                var ordersWithStatus = await orderServices.GetOrderDetails(OrderId);
                return Ok(ordersWithStatus);
            }
            catch (Exception ex)
            {
                return BadRequest("danh sách trống" + ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("Show")]
        public async Task<IActionResult> GetOrdersWithStatus()
        {
            var ordersWithStatus = await orderServices.GetOrder_Statuses();
            return Ok(ordersWithStatus);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            var orderDetails = await orderServices.GetOrderDetails(orderId);

            if (orderDetails == null)
            {
                return BadRequest("Danh sách trống");
            }

            return orderDetails;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/update-status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId,[FromQuery] int newStatus)
        {
            try
            {
               var update= await orderServices.UpdateOrderStatusAndSendEmail(orderId, newStatus);
                   return Ok(update);             
            }
            catch (Exception ex)
            {
                return BadRequest( ex.Message);
            }
        }
    }
}

