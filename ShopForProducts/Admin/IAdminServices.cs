using ShopForProducts.Entities;

namespace ShopForProducts.Admin
{
    public interface IAdminServices
    {
         Task<Account> InitializeAdminAccount();
    }
}