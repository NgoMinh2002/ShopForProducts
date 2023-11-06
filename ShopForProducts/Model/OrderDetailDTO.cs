namespace ShopForProducts.Model
{
    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceTotal { get; set; }
        public int Quantity { get; set; }
    }
}