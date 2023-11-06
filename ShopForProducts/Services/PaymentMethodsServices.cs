using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;
using System;

namespace ShopForProducts.Services
{
    public class PaymentMethodsServices : IPaymentMethodsServices
    {
        private readonly AppDbcontext appDbcontext;
        public PaymentMethodsServices() {
            appDbcontext = new AppDbcontext();
        }
        public async  Task<Payment> AddPayment(string payment_method)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    string paymentMethodLower = payment_method.ToLower();

                    var payment = await appDbcontext.dboPayment.FirstOrDefaultAsync(p =>
                        p.payment_method.ToLower() == paymentMethodLower
                    );
                    if (payment != null)
                    {
                        throw new Exception($"Phương thức thanh toán {payment.payment_method} đã có trong danh sách!");
                    }
                    else
                    {
                        var payments = new Payment()
                        {
                            payment_method = payment_method,
                            status = 1,
                            created_at = DateTime.Now,
                        };
                        appDbcontext.dboPayment.Add(payments);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                        return payments;

                    }
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Lỗi: " + ex.Message);
                }
            }
        }

        public async Task<Payment> DeletePayment(string name_maymet)
        {
            using (var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    string paymentMethodLower = name_maymet.ToLower();

                    var payment = await appDbcontext.dboPayment.FirstOrDefaultAsync(p =>
                        p.payment_method.ToLower() == paymentMethodLower
                    );
                    if (payment == null) {
                        throw new Exception($"Phương thức {payment.payment_method} không có trong danh sách");
                    }
                    else
                    {
                        var resetProuduct = await appDbcontext.dboOrders.Where(p => p.PaymentId ==  payment.PaymentId).ToListAsync();
                        foreach (var product in resetProuduct)
                        {
                            product.PaymentId = null;
                        }
                        appDbcontext.dboPayment.Remove(payment);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                        return payment;
                    }
                }
                catch(Exception ex)
                {
                    transection.Rollback( );
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<Payment>> GetPayment()
        {
            var payment= await appDbcontext.dboPayment.ToListAsync();
            if (payment == null)
            {
                throw new Exception("Danh sách trống");
            }
            else
            {
                return payment;
            }
        }

        public async Task<Payment> UpdatePayment(int id, PaymentModel paymentModel)
        {
            using(var transection = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var payments = await appDbcontext.dboPayment.FirstOrDefaultAsync(pa => pa.PaymentId == id);
                    if(payments == null)
                    {
                        throw new Exception($"Không có thông tin phương thức  này.");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(paymentModel.payment_method))
                        {
                            payments.payment_method = paymentModel.payment_method;
                        }
                        if (paymentModel.status >= 0)
                        {
                            payments.status = (int)paymentModel.status;
                        }
                        payments.updated_at = DateTime.Now;
                        appDbcontext.dboPayment.Update(payments);
                        await appDbcontext.SaveChangesAsync();
                        transection.Commit();
                        return payments;
                    }
                }
                catch(Exception ex) 
                {
                    transection.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
