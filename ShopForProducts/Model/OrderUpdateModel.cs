namespace ShopForProducts.Model
{
    public class OrderUpdateModel
    {
        public int PaymentId { get; set; }
        public string full_name { get; set; }
        public string Email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
       
        /*public IEnumerable<Order_detailModel>? order_detail { get; set; }*/
    }
}
