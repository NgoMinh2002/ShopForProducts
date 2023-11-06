namespace ShopForProducts.Model
{
    public class OrderDetailsDTO
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double OriginalPrice { get; set; }
        public double ActualPrice { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }
}