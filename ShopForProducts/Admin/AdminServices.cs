using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;

namespace ShopForProducts.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly AppDbcontext appDbcontext;
        public AdminServices()
        {
            appDbcontext = new AppDbcontext();
        }

        public async Task<Account> InitializeAdminAccount()
        {
            using (var trans = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingAdmin = await appDbcontext.dboAccounts.FirstOrDefaultAsync(a => a.User_name == "Admin");
                    if (existingAdmin == null)
                    {
                        var defaultDecentralization = await appDbcontext.dboDecentralizations.FirstOrDefaultAsync(d => d.Authority_name == "Admin");
                        if (defaultDecentralization == null)
                        {
                            defaultDecentralization = new Decentralization()
                            {
                                Authority_name = "Admin",
                                created_at = DateTime.Now,
                                updated_at = DateTime.Now
                                
                            };
                            appDbcontext.dboDecentralizations.Add(defaultDecentralization);
                            await appDbcontext.SaveChangesAsync();
                        }

                        var adminAccount = new Account
                        {
                            User_name = "Admin",
                            Email = "anhzkin@gmail.com",
                            Status = 1,
                            PassWord = BCrypt.Net.BCrypt.HashPassword("admin123@"), // Mã hóa mật khẩu
                            DecentralizationId = defaultDecentralization.DecentralizationId,
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now,

                        };
                        appDbcontext.dboAccounts.Add(adminAccount);
                        await appDbcontext.SaveChangesAsync();
                        trans.Commit();
                    }




                    return existingAdmin;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
