using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int? UserId { get; set; }
 
        public int? PaymentId { get; set; }
        [JsonIgnore]
        public  Payment? Payment { get; set; }
        [JsonIgnore]
        public double? original_price { get; set; }
       
        public double? actual_price { get; set; }
        
        public string? full_name { get; set; }
      
        public string? Email { get; set; }
       
        public string? phone { get; set; }
    
        public string? address { get; set; }
     
        public int? Order_statusId { get; set; }
        [JsonIgnore]
        public virtual Order_status? status { get; set; }
        [JsonIgnore]
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
     
        [ForeignKey("UserId")]
        public Account? Account { get; set; }
   
        public IEnumerable<Order_detail> ? order_detail { get; set; }

    }
}
