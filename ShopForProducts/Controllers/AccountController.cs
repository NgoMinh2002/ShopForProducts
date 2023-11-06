using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models;
using ShopForProducts.Admin;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using ShopForProducts.Services;
using System.Security.Claims;

namespace ShopForProducts.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccCountIServices accountService;
        private readonly ManageUserInformation_IServices manageUser;
        private readonly IAuthService _authService;
        private readonly IAdminServices adminServices;
        public AccountController()
        {
            _authService = new AuthService();
            manageUser = new ManageUserInformation_Services();
            accountService = new AccCountServices();
            adminServices = new AdminServices();

        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccounts([FromBody] RegisterAccountModel model)
        {
            try
            {
                var account = await accountService.CreateAccount(model);
                return Ok(new { message = "Tài khoản đã được tạo thành công.", account });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyAccount(string verificationCode)
        {
            try
            {
                var success = await accountService.VerifyAccount(verificationCode);

                if (success)
                {
                    return Ok("Mã xác thực thành công, đăng ký thành công");
                }
                else
                {
                    return BadRequest("Mã xác thực không hợp lệ.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerificationCode([FromQuery] int userid)
        {
            try
            {
                bool result = await accountService.ResendVerificationCode(userid);

                if (result)
                {
                    return Ok("Mã xác thực mới đã được gửi.");
                }
                else
                {
                    return BadRequest("Không thể gửi lại mã xác thực.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] LoginModel model)
        {
            var userModel = await _authService.Authenticate(model.UserName, model.Password);
            if (userModel != null)
            {
                return Ok(userModel); // Trả về model chứa mã thông báo
            }
            return Unauthorized(new { message = "Đăng nhập không thành công." });
        }
        [HttpPost("renewToken")]
        public async Task<IActionResult> RenewToken([FromBody] ToKenModel model)
        {
            try
            {
                var refreshToken = await _authService.RenewToken(model);
                return Ok(refreshToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("secured-page")]
        [Authorize(Roles = "User")]
        public IActionResult SecuredPage()
        {
            // Ghi log về yêu cầu và mã JWT
            var jwt = HttpContext.Request.Headers["Authorization"].ToString();
            // Tiếp tục xử lý phương thức
            return Ok(new { message = "Người dùng có quyền truy cập." });
        }
        [HttpPut("updateUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpadateUser([FromQuery]  string fullname, string phone, string address, string avatar)
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int userIdInt))
                {
                   var User = await manageUser.updateUser(userIdInt,fullname,phone,address,avatar);
                    return Ok("Đã cập nhật thành công thông tin người dùng."+User);
                }
                else
                {
                    return Unauthorized("Người dùng không thể thay đổi tải khoản");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }
        [HttpPut("Changepassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string username, string password, string A_newPassword, string Confirmpassword)
        {
            try
            {
                var changePassword = await manageUser.ChangePassword(username, password, A_newPassword, Confirmpassword);
                return Ok("Đổi Mật khẩu thành công");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Forgotpassword")]
        public async Task<IActionResult> Forgotpassword([FromQuery] string email)
        {
            try
            {
                bool result = await manageUser.Forgotpassword(email);

                if (result)
                {
                    return Ok("Mã xác thực mới đã được gửi.");
                }
                else
                {
                    return BadRequest("Không thể gửi lại mã xác thực.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string resetToken, string newPassword)
        {
            try
            {
                var result = await manageUser.ResetPassword(resetToken, newPassword);

                if (result)
                {
                    return Ok("Đặt lại mật khẩu thành công.");
                }
                else
                {
                    return BadRequest("Không thể đặt lại mật khẩu.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
