using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using ShopForProducts.Services;

namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodsServices paymentMethodsServices;
        public PaymentMethodsController()
        {
            paymentMethodsServices = new PaymentMethodsServices();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddPaymetMethods")]
        public async Task<IActionResult> AddPaymetMethods([FromBody] string name_maymet)
        {
            try
            {
                await paymentMethodsServices.AddPayment(name_maymet);
                return Ok("Phương thức đã được thêm.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Lỗi khi thêm phương thức: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatPayment")]
        public async Task<IActionResult> UpdatPayment([FromRoute] int id,[FromBody]  PaymentModel paymentModel)
        {
            try
            {
                var updatedProduct = await paymentMethodsServices.UpdatePayment(id, paymentModel);

                return Ok(new { message = "Cập nhật phương thức thanh toán thành công.", product = updatedProduct });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DetelePayment/{name_maymet}")]
        public async Task<IActionResult> DetelePayment(string name_maymet)
        {
            try
            {
                await paymentMethodsServices.DeletePayment(name_maymet);
                return Ok("Đã xóa  phương thức này.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Xóa phương thức này bị lỗi: " + ex.Message);
            }
        }
        [HttpGet("ShowPayment")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> ShowPayment()
        {
            var Payment = await paymentMethodsServices.GetPayment();
            if (Payment == null)
            {
                return NotFound("Danh sách trống");
            }
            return Ok(Payment);
        }
    }
}
