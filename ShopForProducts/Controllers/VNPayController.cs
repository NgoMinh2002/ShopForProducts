using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices.VNPay;


namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public VNPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        [HttpPost("VnPay")]
        public async Task<IActionResult> VNPay([FromQuery] int orderId)
        {
            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(orderId, HttpContext);
                return Ok(paymentUrl);

            }
            catch (Exception ex)
            {
                return BadRequest("danh sách trống" + ex.Message);
            }

        }
        [HttpPost("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
        [HttpGet("createPayment")]
        public IActionResult CreatePayment([FromQuery] int orderID, [FromQuery] int paymentID)
        {
            return NewMethod(orderID, paymentID);

        }

        private IActionResult NewMethod(int orderID, int paymentID)
        {
            try
            {
                var paymentUrl = _vnPayService.CreatePaymen(orderID, paymentID, HttpContext);
                return Ok(paymentUrl);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
