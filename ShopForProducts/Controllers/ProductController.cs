using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using ShopForProducts.Services;
namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        private readonly IProductSevrives productSevrives;
        public ProductController()
        {
            productSevrives = new ProductSevrives();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePorduct{nameproduct}")]
        public async Task<IActionResult> Deteleproducttype(string nameproduct)
        {
            try
            {
                await productSevrives.DeteleProducts(nameproduct);
                return Ok("Đã xóa  sản phẩm này.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Xóa sản phẩm bị lỗi: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel model)
        {
            try
            {

                await productSevrives.AddProduct(model); ;
                return Ok("Sản phẩm đã được thêm.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Lỗi khi thêm  sản phẩm: " + ex.Message);
            }
        }
        
        [HttpGet("ShowProduct")]
        public async Task<IActionResult> ShowPeoduct()
        {
            var product = await productSevrives.Showproduct();
            return Ok(product);
        }
        [HttpGet("ShowView")]
        public async Task<IActionResult> ShowView(int productId)
        {
            try
            {

                var product = await productSevrives.GetProductViews(productId);
                return Ok(product);
                
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest( ex.Message);
            }
        }
        [HttpGet("Outstandingproduct")]
        public async Task<IActionResult> Outstandingproduct()
        {
            var product = productSevrives.GetOutstandingproduct(5);
            return Ok(product);
            // Lấy 5 sản phẩm nổi bật


        }
        [HttpGet("getinfo")]
        public async Task<IActionResult> GetProductInfoByName([FromQuery] string nameproduct)
        {
            try
            {
                var product = await productSevrives.GetProduct(nameproduct);
                return Ok(product);


            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi hiện thị  sản phẩm: " + ex.Message);
            }
        }
        [HttpGet("{nameproduct}")]
        public async Task<IActionResult> GetProduct(string nameproduct)
        {
            try
            {
                var product = await productSevrives.GetProduct(nameproduct);

                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm.");
                }

                var relatedProducts = await productSevrives.GetRelatedProducts(nameproduct);
              

                var response = new
                {
                    Message = "Đây là sản phẩm gốc",
                    OriginalProduct =  product,
                    Messages="Đây là sản phẩm liên quan",
                    RelatedProducts = relatedProducts
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi không hiện thị sản phẩm: " + ex.Message);
            }
            /* try
             {
                 var relatedProducts = await productSevrives.GetRelatedProducts(nameproduct, maxRelatedProducts: 10);

                 return Ok(relatedProducts);
             }
             catch (Exception ex)
             {
                 return BadRequest("Lỗi không hiển thị sản phẩm liên quan: " + ex.Message);
             }*/
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{nameproduct}")]
        public async Task<IActionResult> UpdateProductInfo(string nameproduct, [FromQuery] Updateprpcuct productupdate)
        {
            try
            {
                /*var product = await productSevrives.GetProduct(nameproduct);*/
                var updatedProduct = await productSevrives.UpdateProduct(nameproduct, productupdate);

                return Ok(new { message = "Cập nhật sản phẩm thành công.", product = updatedProduct });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
