using ShopForProducts.Entities;
using ShopForProducts.Model;
namespace ShopForProducts.IServices
{
    public interface IPaymentMethodsServices
    {
        Task<Payment> AddPayment(string payment_method);
        Task<Payment> UpdatePayment(int id, PaymentModel paymentModel );
        Task<Payment> DeletePayment(string name_maymet);
        Task<List<Payment>> GetPayment();
        
    }
}
