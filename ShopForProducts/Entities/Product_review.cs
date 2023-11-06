using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopForProducts.Entities
{
    public class Product_review
    {
        [Key]
        public int Product_reviewId { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public Account?  Account { get; set; }
        public string? content_rated { get; set; }

        public int? point_evaluation { get; set; }
        public string? content_seen { get; set; }

        public int? status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }

    }
}
