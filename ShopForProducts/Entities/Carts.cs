using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
    public class Carts
    {
        [Key]

        public int CartId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
     
        [ForeignKey("UserId")]
        public Account ? Account { get; set; }
        [JsonIgnore]

        public DateTime ?created_at { get; set; }
        [JsonIgnore]

        public DateTime ?updated_at { get; set; }
        [JsonIgnore]

        public IEnumerable<Cart_item>? Cart_Items { get; set; }
        
    }
}
