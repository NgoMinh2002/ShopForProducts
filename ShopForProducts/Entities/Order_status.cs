using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class Order_status
    {
        [Key]
        public int Order_statusId { get; set; }
        public string status_name { get; set; }
        public IEnumerable<Order>? orders { get; set; }

    }
}
