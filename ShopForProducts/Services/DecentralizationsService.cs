using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;

namespace ShopForProducts.Services
{
    public class DecentralizationsService : IDecentralizationsService
    {
        private readonly AppDbcontext appDbcontext;
        public DecentralizationsService()
        {
            appDbcontext = new AppDbcontext();
        }
        public async Task<Decentralization> Create(string name)
        {
            using (var trans = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var authority = await appDbcontext.dboDecentralizations.FirstOrDefaultAsync(a => a.Authority_name == name);
                    if (authority != null)
                    {
                        throw new Exception("Quyền hạng này đã có");

                    }
                    else
                    {
                        var authoritys = new Decentralization()
                        {
                            Authority_name = name,
                            created_at = DateTime.Now,

                        };
                        appDbcontext.dboDecentralizations.Add(authoritys);
                        await appDbcontext.SaveChangesAsync();
                        trans.Commit();
                        return authoritys;

                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<Decentralization> Delete(string name)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var Authority = await appDbcontext.dboDecentralizations.FirstOrDefaultAsync(p => p.Authority_name == name);
                    if (Authority == null)
                    {
                        throw new Exception($"{name} Không có quyền hàng này!");
                    }
                    else
                    {
                        var reset = await appDbcontext.dboAccounts.Where(r => r.DecentralizationId == Authority.DecentralizationId).ToListAsync();
                        foreach (var product in reset)
                        {
                            product.DecentralizationId = null;
                        }
                        appDbcontext.dboDecentralizations.Remove(Authority);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                    }
                    return Authority;

                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Xóa thất bại" + ex.Message);
                }
            }
        }

        public async Task<List<Decentralization>> GetDecentralization()
        {
            try
            {
                var Authority = await appDbcontext.dboDecentralizations.ToListAsync();
                if (Authority == null)
                {
                    throw new Exception("Danh sách rỗng");
                }
                else
                {
                    return Authority;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi:" + ex.Message);
            }
        }
    }
}
