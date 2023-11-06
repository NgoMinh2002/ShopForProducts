namespace ShopForProducts.Model.VNPay
{
    public class ErrorViewModel
    {
        public string ? ErquestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(ErquestId);
    }
}
