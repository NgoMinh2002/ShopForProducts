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
    public class EvaluateController : ControllerBase
    {
        private readonly IProductreviewsServices productreviews;
        public EvaluateController()
        {
            productreviews = new ProductreviewsServices();
        }
        [Authorize(Roles = "User")]
        [HttpPost("submit-review")]
        public async Task<IActionResult> SubmitProductReview([FromBody] ProductreviewsModel modle)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
                {
                    if (await productreviews.CanSubmitProductReview(userIdInt, modle.ProductId))
                    {
                        await productreviews.SubmitProductReview(userIdInt,modle);
                        return Ok("Cảm ơn bạn đã đánh giá sản phẩm.");
                    }
                    else
                    {
                        return BadRequest("Không thể đánh giá sản phẩm.");
                    }
                }
                else
                {
                    return BadRequest("Đánh giá thất bại, vui lòng đăng nhập");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
               
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("ShowReview")]
        public async Task<IActionResult> ShowReview()
        {
            var review = productreviews.Get_Review();
            return Ok(review);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeteleReview")]
        public async Task<IActionResult> DeteleReview(int Product_reviewId)
        {
            try
            {
                await productreviews.DeteleReview(Product_reviewId);
                return Ok("Đã xóa loại sản phẩm này.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Xóa loại sản phẩm bị lỗi: " + ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPut("UpdateView")]
        public async Task<IActionResult> UpdateView(int Product_reviewId, [FromBody] updateView modle)
        {
            try
            {

                var updateview = await productreviews.Updateproduct_Review(Product_reviewId, modle);

                return Ok(new { message = "Cập nhật đánh giá thành công.", view = updateview });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
