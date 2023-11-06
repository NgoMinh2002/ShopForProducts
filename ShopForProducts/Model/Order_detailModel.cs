using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Model
{
    public class Order_detailModel
    {
       
       /* public int Order_detailId { get; set; }*/
        /*public int? OrderId { get; set; }*/
        public int? ProductId { get; set; }
        /*public double? price_total { get; set; }*/
        public int? quantity { get; set; }


    }
}
