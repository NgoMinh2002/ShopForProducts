using ShopForProducts.Entities;

namespace ShopForProducts.Model
{
    public class ProductModel
    {
        public string? name_product { get; set; }
        public int? Product_typeId { get; set; } 
        public double? price { get; set; }
        public string? avartar_image_product { get; set; } 
        public string? title { get; set; } 
        public int? discount { get; set; }     
    }
}
