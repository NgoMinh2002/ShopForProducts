using ShopForProducts.Entities;

namespace ShopForProducts.IServices
{
    public interface ManageUserInformation_IServices
    {
        Task<Account> updateUser(int userId, string fullname, string phone, string address, string avatar /*int DecentralizationId*/);
        Task<Account> ChangePassword(string username, string password, string A_newPassword, string Confirmpassword );
        Task<bool> Forgotpassword(string email);
        Task<bool> ResetPassword( string resetToken, string newPassword);
    }
}
