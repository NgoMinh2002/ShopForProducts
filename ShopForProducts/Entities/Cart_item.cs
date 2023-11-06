using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
    public class Cart_item
    {
        [Key]
        public int Cart_itenId { get; set; }
        [JsonIgnore]
        public int? ProductId { get; set; }
        [JsonIgnore]
        public Product? product { get; set; }
        [JsonIgnore]
        public int? CartId { get; set; }
        [JsonIgnore]
        public Carts? cart { get; set; }
        [JsonIgnore]
        public int ?quantity { get; set; }
        [JsonIgnore]
        public DateTime ?created_at { get; set; }
        [JsonIgnore]
        public DateTime ?updated_at { get; set; }
    }
}