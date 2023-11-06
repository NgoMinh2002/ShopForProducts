

namespace ShopForProducts.Model
{
    public class OrderModel
    {
        public int PaymentId { get; set; }
       /* public int? UserId { get; set; }*/
        /*  public double? original_price { get; set; }
          public double ?actual_price { get; set; }*/
        public string full_name { get; set; }
        public string Email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        /*public int? Order_statusId { get; set; }*/
        public IEnumerable<Order_detailModel>? order_detail { get; set; }

    }
}
