using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
namespace ShopForProducts.Services
{
    public class ManageUserInformation_Services : ManageUserInformation_IServices
    {
        private readonly AppDbcontext appDbcontext;
        public ManageUserInformation_Services()
        {
            appDbcontext = new AppDbcontext();
        }
        public async Task<Account> ChangePassword(string username, string password, string A_newPassword, string Confirmpassword)
        {
            var Username = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.User_name == username);
            bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(password, Username.PassWord);
            if (Username != null && isOldPasswordValid)
            {

                if (A_newPassword == Confirmpassword)
                {
                    Username.PassWord = BCrypt.Net.BCrypt.HashPassword(A_newPassword);
                    Username.updated_at = DateTime.UtcNow;
                    appDbcontext.dboAccounts.Update(Username);
                    await appDbcontext.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Mật khẩu không khớp");
                }
            }
            else
            {
                throw new InvalidOperationException("Username Hoặc PassWord Không đúng");
            }
            return Username;
        }
        public async Task<bool> Forgotpassword(string email)
        {
            var account = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account != null)
            {
                string resetToken = GenerateResetToken();
                account.ResetPasswordToken = resetToken;
                account.ResetPasswordTokenExpiry = DateTime.Now.AddMinutes(1);
                account.updated_at = DateTime.Now;
                appDbcontext.dboAccounts.Update(account);
                await appDbcontext.SaveChangesAsync();
                SendVerificationEmail(account.Email, resetToken);
                if (account.ResetPasswordTokenExpiry <= DateTime.Now)
                {
                    account.ResetPasswordToken = null;
                    account.ResetPasswordTokenExpiry = null;
                    account.updated_at = DateTime.Now;
                    appDbcontext.dboAccounts.Update(account);
                    await appDbcontext.SaveChangesAsync();
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ResetPassword(string resetToken, string newPassword)
        {
            var account = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.ResetPasswordToken == resetToken);

            if (account != null)
            {
                if (account.ResetPasswordTokenExpiry.HasValue && account.ResetPasswordTokenExpiry > DateTime.Now)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    account.PassWord = hashedPassword; // Cập nhật mật khẩu mới
                    account.ResetPasswordToken = null; // Đặt lại mã xác thực tạm thời
                    account.ResetPasswordTokenExpiry = null; // Hủy thời hạn mã xác thực
                    account.updated_at = DateTime.Now;
                    appDbcontext.dboAccounts.Update(account);
                    await appDbcontext.SaveChangesAsync();

                    return true;
                }
                else
                {// Nếu thời hạn đã hết hạn, hủy mã xác thực và thời hạn
                    account.ResetPasswordToken = null;
                    account.ResetPasswordTokenExpiry = null;
                    account.updated_at = DateTime.Now;
                    appDbcontext.dboAccounts.Update(account);
                    await appDbcontext.SaveChangesAsync();
                }
            }
            return false; // Trả về false nếu mã xác thực không đúng hoặc hết hạn
        }
        private string GenerateResetToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            const int codeLength = 6;
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
                message.Subject = "Xác Thực mật khẩu";
                message.Body = new TextPart("plain")
                {
                    Text = "Mã xác thực của bạn: " + verificationCode + "\n https://localhost:7191/swagger/index.html"

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
        public async Task<Account> updateUser(int userId, string fullname, string phone, string address, string avatar/*, int DecentralizationId*/)
        {
            var User = await appDbcontext.dboAccounts.FindAsync(userId);
            if (User != null)
            {
                User.Phone = phone;
                User.Address = address;
                User.FullName = fullname;
                User.Avatar_url = avatar;
                User.updated_at = DateTime.Now;
                /*User.DecentralizationId = DecentralizationId;*/
                appDbcontext.dboAccounts.Update(User);
                await appDbcontext.SaveChangesAsync();
            }
            return User;
        }
    }
}
