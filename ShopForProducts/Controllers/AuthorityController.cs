using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopForProducts.IServices;
using ShopForProducts.Services;

namespace ShopForProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthorityController : ControllerBase
    {
        private readonly IDecentralizationsService _decentralizationsService;

        public AuthorityController()
        {
            _decentralizationsService = new DecentralizationsService();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CraeteAuthority")]
        public async Task<IActionResult> Craeteauthority([FromBody] string name)
        {
            try
            {
                var authority = await _decentralizationsService.Create(name);
                return Ok(authority);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Authority_name}")]
        public async Task<IActionResult> Deteleproducttype(string Authority_name)
        {
            try
            {
                await _decentralizationsService.Delete(Authority_name);
                return Ok("Đã xóa quyền hạng này.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest("Xóa quyền hạng bị lỗi: " + ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Show_Authority")]
        public async Task<IActionResult> ShowAuthority()
        {
            var Show_Authority = await _decentralizationsService.GetDecentralization();
            return Ok(Show_Authority);
        }
    }
}
