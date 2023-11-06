using ShopForProducts.Entities;
using ShopForProducts.Model.VNPay;

namespace ShopForProducts.IServices.VNPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(int orderId, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
      
        string CreatePaymen(int OrderId, int paymentId, HttpContext context);
    }
}
