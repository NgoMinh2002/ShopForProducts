using ShopForProducts.Entities;
using ShopForProducts.Model;

namespace ShopForProducts.IServices
{
    public interface IAuthService
    {
        Task<SecureData> Authenticate(string username, string password);
        Task<ToKenModel> GenerateJwtToken(Account account);

         Task<ToKenModel> RenewToken(ToKenModel model);
    }
}
