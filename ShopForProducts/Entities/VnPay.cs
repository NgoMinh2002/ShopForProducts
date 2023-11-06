using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class VnPay
    {
        [Key]   
        public int Id { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string VnPayResponseCode { get; set; } = string.Empty;
    }
}