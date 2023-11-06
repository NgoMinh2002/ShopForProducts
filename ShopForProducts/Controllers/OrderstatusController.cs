using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.Services;
using ShopForProducts.IServices;
using ShopForProducts.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrderstatusController : ControllerBase
    {
        public readonly IOrderstatusServices orderstatusServices;
        public OrderstatusController() {
            orderstatusServices = new OrderstatusServices();
        }
        [HttpPost("AddOrderStatus")]
        public async Task<IActionResult> AddOrderStatus([FromBody] string nameStatus) {
            try
            {

                await orderstatusServices.AddOrer_Status(nameStatus);
                return Ok("Trạng thái đặt hàng đã được thêm.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Lỗi khi thêm trạng thái đặt hàng: " + ex.Message);
            }
        }
        [HttpDelete("{nameStatus}")]
        public async Task<IActionResult> Deteleproducttype(string nameStatus)
        {
            try
            {
                await orderstatusServices.DeleteOrder_status(nameStatus);
                return Ok("Trạng thái đã được xóa.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Không thể xóa trạng thái này : " + ex.Message);
            }
        }
    }
}
