using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class Product_image
    {
        [Key]
        public int Product_imageId { get; set; }
        public string title { get; set; }
        public string image_product { get; set; }
        public int ProductiId { get; set; }
        public Product? product { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
