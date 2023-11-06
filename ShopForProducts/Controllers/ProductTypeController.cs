using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices;
using ShopForProducts.Services;
namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeSevrives productTypeSevrives;
        public ProductTypeController()
        {
            productTypeSevrives = new ProductTypeSevrives();
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public async Task<IActionResult> Showproducttype()
        {
            var product = await productTypeSevrives.Showproducttype();
            if (product == null)
            {
                // Thông báo rằng danh sách sản phẩm trống
                return NotFound("Danh sách sản phẩm trống");
            }
            // Trả về sản phẩm hoặc dữ liệu cụ thể khác
            return Ok(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Add_ProductType")]
        public async Task<IActionResult> Addproducttype([FromBody] string ProductType_name)
        {
            try
            {
                await productTypeSevrives.Addproducttype(ProductType_name);
                return Ok("Loại sản phẩm đã được thêm.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Lỗi khi thêm loại sản phẩm: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Updateproducttype")]
        public async Task<IActionResult> Updateproducttype([FromBody] int ProducttypeId, string ProductType_name)
        {
            try
            {
                await productTypeSevrives.Updateproducttype(ProducttypeId, ProductType_name);
                return Ok("Loại sản phẩm đã được cập nhật mới.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Lỗi khi cập thật sản phẩm loại sản phẩm: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{ProductType_name}")]
        public async Task<IActionResult> Deteleproducttype(string ProductType_name)
        {
            try
            {
                await productTypeSevrives.Deleteproducttype(ProductType_name);
                return Ok("Đã xóa loại sản phẩm này.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Xóa loại sản phẩm bị lỗi: " + ex.Message);
            }
        }
    }
}
