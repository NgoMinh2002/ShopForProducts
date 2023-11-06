using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class Order_detail
    {
        [Key]
        public int Order_detailId { get; set; }
        public int OrderId { get; set; }


        public virtual Order? Order { get; set; }
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public double? price_total { get; set; }

        public int? quantity { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

    }
}
