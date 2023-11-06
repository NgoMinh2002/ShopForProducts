using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string? payment_method { get; set; }
        public int? status { get; set; }
        public DateTime ?created_at { get; set; }
        public DateTime? updated_at { get; set; }
        [JsonIgnore]
        public IEnumerable<Order>? orders { get; set; }

    }
}
