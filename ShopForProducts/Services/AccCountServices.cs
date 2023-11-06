using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;

namespace ShopForProducts.Services
{
    public class AccCountServices : AccCountIServices
    {
        private readonly AppDbcontext appDbcontext;
        private readonly IConfiguration _configuration;
        public AccCountServices()
        {
            appDbcontext = new AppDbcontext();

        }
        public AccCountServices(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public async Task<Account> CreateAccount(RegisterAccountModel model)
        {
            var existingAccount = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existingAccount != null)
            {
                // Trả về thông báo lỗi nếu tài khoản đã tồn tại
                throw new InvalidOperationException("Email đã được sử dụng cho tài khoản khác.");
            }
            var existingAccounts = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.User_name == model.User_name);
            if (existingAccount != null)
            {
                // Trả về thông báo lỗi nếu tài khoản đã tồn tại
                throw new InvalidOperationException("UserName đã tồn tại.");
            }
            if (model.PassWord != model.Confirmpassword)
            {
                // Trả về thông báo lỗi nếu xác nhận mật khẩu không khớp
                throw new InvalidOperationException("Mật khẩu và xác nhận mật khẩu không khớp.");
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PassWord);
            var verificationCode = GenerateVerificationCode();
            var account = new Account
            {
                User_name = model.User_name,
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                PassWord = hashedPassword,
                Status = 0,
                DecentralizationId = 2,
                ResetPasswordToken = verificationCode,
                ResetPasswordTokenExpiry = DateTime.Now.AddMinutes(10),
                // Lưu mật khẩu đã mã hóa
                created_at = DateTime.UtcNow,
            };
            appDbcontext.dboAccounts.Add(account);
            await appDbcontext.SaveChangesAsync();
            string userEmailAddress = account.Email;
            // Gửi email xác thực
            // Tạo mã xác thực
            SendVerificationEmail(userEmailAddress, verificationCode);
            return account;
        }
        private string GenerateVerificationCode()
        {
            // Triển khai logic tạo mã xác thực, ví dụ: tạo một chuỗi ngẫu nhiên
            string verificationCode = GenerateRandomCode();
            return verificationCode;
        }
        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int codeLength = 8;
            var random = new Random();
            var code = new string(Enumerable.Repeat(chars, codeLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return code;
        }
        public void SendVerificationEmail(string userEmailAddress, string verificationCode)
        {
            string smtpServer = "smtp.gmail.com"; // Thay thế bằng máy chủ SMTP của bạn
            int smtpPort = 587; // Port của máy chủ SMTP
            string fromEmail = "anhkunvipzz@gmail.com"; // Địa chỉ email nguồn
            string password = "yvin mhtv yipm gatv\r\n"; // Mật khẩu của địa chỉ email nguồn

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, false);
                client.Authenticate(fromEmail, password);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your Name", fromEmail)); // Địa chỉ email nguồn
                message.To.Add(new MailboxAddress("", userEmailAddress)); // Sử dụng địa chỉ email từ biến userEmailAddress
                message.Subject = "Xác thực tài khoản";
                message.Body = new TextPart("plain")
                {
                    Text = "Mã xác thực của bạn: " + verificationCode
                };
                try
                {
                    client.Send(message);
                    Console.WriteLine("Email xác thực đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi gửi email: " + ex.Message);
                }
                client.Disconnect(true);
            }
        }
        public async Task<bool> VerifyAccount(string verificationCode)
        {
            var account = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.ResetPasswordToken == verificationCode);

            if (account != null && (account.ResetPasswordTokenExpiry.HasValue && account.ResetPasswordTokenExpiry > DateTime.Now))
            {
                // Mã xác thực khớp, cập nhật trạng thái tài khoản
                account.Status = 1; // Đặt trạng thái xác thực thành 1 (hoặc giá trị khác bạn chọn)
                account.ResetPasswordToken = null; // Đặt lại mã xác thực
                account.ResetPasswordTokenExpiry = null; // Đặt lại thời hạn mã xác thực
                appDbcontext.dboAccounts.Update(account);
                await appDbcontext.SaveChangesAsync();
                return true; // Trả về true để xác thực thành công
            }
            return false; // Trả về false để xác thực không thành công
        }
        public async Task<bool> ResendVerificationCode(int userid)
        {
            var account = await appDbcontext.dboAccounts.FindAsync(userid);
            if (account != null)
            {
                // Tạo mã xác thực mới
                string newVerificationCode = GenerateVerificationCode();
                // Cập nhật mã xác thực mới vào tài khoản
                account.ResetPasswordToken = newVerificationCode;
                account.ResetPasswordTokenExpiry = DateTime.Now.AddMinutes(1);
                appDbcontext.dboAccounts.Update(account);
                await appDbcontext.SaveChangesAsync();
                // Gửi lại email xác thực với mã mới
                SendVerificationEmail(account.Email, newVerificationCode);
                return true; // Trả về true để thông báo gửi lại mã xác thực thành công
            }
            return false; // Trả về false nếu không tìm thấy tài khoản
        }
    }
}