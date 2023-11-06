using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class Product_type
    {
        [Key]
        public int Product_typeId { get; set; }
        public string name_product_type { get; set; }
        // public string image_type_product {  get; set; }
        public DateTime ?created_at { get; set; }
        public DateTime ?updated_at { get; set; }
        public IEnumerable<Product>? products { get; set; }
    }
}
