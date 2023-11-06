using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopForProducts.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbcontext appDbcontext;


        public AuthService()
        {
            appDbcontext = new AppDbcontext();

        }

        public async Task<SecureData> Authenticate(string username, string password)
        {
            var account = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.User_name == username);
            bool accountpass = BCrypt.Net.BCrypt.Verify(password, account.PassWord);
            if (account != null && accountpass)
            {
                var token = await GenerateJwtToken(account);

                // Tạo một đối tượng UserModel và gán mã thông báo
                var userModel = new SecureData
                {
                    UserId = account.UserId,
                    UserName = account.User_name,
                    Email = account.Email,
                    authority = (int)account.DecentralizationId,
                    Token = token // Gán mã thông báo vào trường Token
                };
                return userModel;
            }
            return null;
        }

        public async Task<ToKenModel> GenerateJwtToken(Account account)
        {
            var key = "u*!?}jXBa9eL#7=:~2@Tf_w3t;yN-nqM"; // Thay đổi thành khóa bí mật của bạn
            var issuer = "MinhAnh"; // Thay đổi thành Issuer của bạn
            var audience = "YourAudience"; // Thay đổi thành Audience của bạn

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                     {
                    /*new Claim(ClaimTypes.Name,account.FullName),*/
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserName",account.User_name),
                    new Claim("Id",account.UserId.ToString())
                     /*new Claim(ClaimTypes.NameIdentifier, account.UserId.ToString()),
                     new Claim(ClaimTypes.Name, account.User_name),
                     new Claim(ClaimTypes.Email, account.Email),*/
                };

            var userRoles = appDbcontext.dboDecentralizations
            .Where(d => d.DecentralizationId == account.DecentralizationId)
            .Select(d => d.Authority_name)
            .ToList();

            // Thêm các quyền vào Claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                      issuer: issuer,
                      audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(30),
                    signingCredentials: credentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken()
            {
                Id =Guid.NewGuid(),
                JwtId = token.Id,
                UserId = account.UserId,
                Token = refreshToken,
                IsUsed =false,
                IsRevoked = false,
                IssuedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddDays(30),
            };
            await appDbcontext.refreshTokens.AddAsync(refreshTokenEntity);
            await appDbcontext.SaveChangesAsync();
            return new ToKenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
         
        }

        public async Task<ToKenModel> RenewToken(ToKenModel model)
        {
            try
            {
                var key = "u*!?}jXBa9eL#7=:~2@Tf_w3t;yN-nqM"; // Thay đổi thành khóa bí mật của bạn
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyBytes = Encoding.UTF8.GetBytes(key);
                var tokenValidateParam = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };

                // Xác thực AccessToken
                SecurityToken accessToken;
                var principal = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out accessToken);
                if (accessToken is not JwtSecurityToken securityToken)
                {
                    throw new Exception("AccessToken không hợp lệ");
                }

                // Kiểm tra mã thông báo (JWT ID) của AccessToken
                var accessTokenJti = securityToken.Id;

                // Tìm Refresh Token trong cơ sở dữ liệu
                var storedToken = await appDbcontext.refreshTokens.FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    throw new Exception("Mã thông báo làm mới không tồn tại");
                }

                // Kiểm tra xem Refresh Token đã sử dụng hoặc bị thu hồi chưa
                if (storedToken.IsUsed)
                {
                    throw new Exception("Mã thông báo làm mới đã được sử dụng");
                }
                if (storedToken.IsRevoked)
                {
                    throw new Exception("Mã thông báo làm mới đã bị thu hồi");
                }

                // Kiểm tra xem mã thông báo (JWT ID) của AccessToken có khớp với mã thông báo của Refresh Token
                if (storedToken.JwtId != accessTokenJti)
                {
                    throw new Exception("Mã thông báo không khớp");
                }

                // Đánh dấu Refresh Token đã sử dụng và cập nhật trong cơ sở dữ liệu
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                appDbcontext.Update(storedToken);
                await appDbcontext.SaveChangesAsync();

                // Tạo mới AccessToken và Refresh Token
                var user = await appDbcontext.dboAccounts.SingleOrDefaultAsync(nd => nd.UserId == storedToken.UserId);
                var token = await GenerateJwtToken(user);

                return new ToKenModel
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }

        private string GenerateRefreshToken()
        {
            var ramdom = new byte[32];
            using ( var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(ramdom);
                return Convert.ToBase64String(ramdom);
            }
        }
        

    }
}