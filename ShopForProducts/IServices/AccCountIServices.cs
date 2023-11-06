using Microsoft.AspNetCore.Mvc;
using ShopForProducts.Entities;
using ShopForProducts.Model;

namespace ShopForProducts.IServices
{
    public interface AccCountIServices
    {
        Task<Account> CreateAccount(RegisterAccountModel model);
        Task<bool> VerifyAccount( string verificationCode);
        Task<bool> ResendVerificationCode(int UserId);

    }
}
