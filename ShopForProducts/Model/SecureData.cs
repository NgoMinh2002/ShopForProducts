namespace ShopForProducts.Model
{
    public class SecureData
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int authority { get; set; }
        public object Token { get; set; }

    }
}
