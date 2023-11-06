using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;

namespace ShopForProducts.Services
{
    public class OrderstatusServices : IOrderstatusServices
    {
        private readonly AppDbcontext appDbcontext;
        public OrderstatusServices() {
            appDbcontext = new AppDbcontext();
        }
        public async Task<Order_status> AddOrer_Status(string nameStatus)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var OrderStatus= await appDbcontext.dboOrder_status.FirstOrDefaultAsync(or=>or.status_name==nameStatus);
                    if(OrderStatus!=null)
                    {
                        throw new Exception("Trạng thái đã tồn tại");
                    }
                    else
                    {
                        var Orderstatuss = new Order_status()
                        {
                            status_name = nameStatus,
                            
                        };
                        appDbcontext.dboOrder_status.Add(Orderstatuss);
                        await appDbcontext.SaveChangesAsync();
                        
                        transection.Commit();
                        return Orderstatuss;
                    }
                }
                catch(Exception ex)
                {
                    transection.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public Task<Order_status> AupdateOrder_Status(Order_status status)
        {
            throw new NotImplementedException();
        }

        public async Task<Order_status> DeleteOrder_status(string nameStatus)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try {
                 var status = await  appDbcontext.dboOrder_status.FirstOrDefaultAsync(or => or.status_name==nameStatus);
                    if(status==null)
                    {
                      throw new Exception("Trạng thái không tồn tại");
                    }
                    else
                    { var  resetOrder = await appDbcontext.dboOrders.Where(or=> or.Order_statusId == status.Order_statusId).ToListAsync();
                        foreach (var order in resetOrder)
                        {
                            order.Order_statusId = null;
                        }
                        appDbcontext.dboOrder_status.Remove(status);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                    }
                    return status;
                }
                catch (Exception ex)
                { transection.Rollback();
                    throw new Exception("Lỗi xóa: "+ex.Message);

                }
            }
        }
    }
}
