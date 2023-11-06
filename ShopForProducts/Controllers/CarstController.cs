using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using ShopForProducts.Services;
using System.Security.Claims;

namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarstController : ControllerBase
    {
        private readonly ICartServices cartServices;
        public CarstController()
        {
            cartServices = new CartsServices();
        }
        [Authorize(Roles = "User")]
        [HttpPost("CreateCarts")]
        public async Task<IActionResult> CreateCarts([FromBody] Carts_ItemModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
                {
                    await cartServices.CreateCarts(userIdInt, model.ProductId, model.quantity);
                    return Ok("Sản phẩm đã được thêm vào giỏ hàng.");
                }
                else
                {
                    return Unauthorized("Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost("Order/{cartId}")]
        public async Task<IActionResult> CeateBayCart(int cartId,[FromBody] OrderUpdateModel orderModel) {
            try
            {
                var order = cartServices.CreateOrderFromCart(cartId,orderModel); 
                return Ok("Đã tạo đơn hàng thành công");
                
            }catch (Exception ex)
            {
                throw new Exception("Đặt hàng thất bại:" +ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("DeleteCarts/{CartsId}")]
        public async Task<IActionResult> DeleteCart(int CartsId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                // Gọi phương thức để xóa giỏ hàng của người dùng
                await cartServices.DeteleCarts(CartsId);

                return Ok("Giỏ hàng đã được xóa.");
            }
            else
            {
                return Unauthorized("Vui lòng đăng nhập để xóa giỏ hàng.");
            }
        }

        [Authorize(Roles ="User")]
        [HttpGet("Carts")]
        public async Task<IActionResult> Show_Carst()
        {
            try
            {
                var cartsUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(cartsUserId) && int.TryParse(cartsUserId, out int userId))
                {
                    var showcarst = await cartServices.GetCarts(userId);
                    return Ok(showcarst);
                }
                else
                {
                    return BadRequest("Danh sách trống");
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

