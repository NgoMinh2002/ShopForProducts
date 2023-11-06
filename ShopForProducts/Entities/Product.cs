using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
  
    public class Product
    {
        [Key] // Thuộc tính này là khóa chính cho thực thể Product.
        public int ProductId { get; set; } // Định danh duy nhất cho mỗi sản phẩm.

        [JsonIgnore]
        public int ? Product_typeId { get; set; } // Khóa ngoại để liên kết với loại sản phẩm.
        [JsonIgnore]
        public Product_type? product_Type { get; set; } // Thuộc tính dẫn hướng đến Product_type liên quan. Có thể là null.
        [JsonIgnore]
        public string ? name_product { get; set; } // Tên của sản phẩm.
        [JsonIgnore]
        public double ? price { get; set; }
        public string? avartar_image_product { get; set; } // URL hoặc đường dẫn đến hình ảnh đại diện của sản phẩm.
        [JsonIgnore]
        public string? title { get; set; } // Mô tả hoặc tiêu đề của sản phẩm.
        [JsonIgnore]
        public int ? discount { get; set; } // Giảm giá được áp dụng cho sản phẩm.
        [JsonIgnore]
        public int? status { get; set; } // Trạng thái của sản phẩm (sẵn sàng, hết hàng, v.v.).
        [JsonIgnore]
        public int? number_of_views { get; set; } // Số lượng lượt xem hoặc truy cập của sản phẩm.
        [JsonIgnore]
        public DateTime ?created_at { get; set; } // Thời điểm sản phẩm được tạo.
        [JsonIgnore]
        public DateTime? updated_at { get; set; } // Thời điểm sản phẩm được cập nhật lần cuối.

        // Mối quan hệ một-nhiều với Product_image. Có thể là null.
        /*public IEnumerable<Product_image>? products { get; set; }*/

        // Mối quan hệ một-nhiều với Product_review. Có thể là null.
        [JsonIgnore]
        public IEnumerable<Product_review>? product_Reviews { get; set; }

        // Mối quan hệ một-nhiều với Cart_item. Có thể là null.
        [JsonIgnore]
        public IEnumerable<Cart_item>? cart_Items { get; set; }
        [JsonIgnore]
        // Mối quan hệ một-nhiều với Order_detail.
        public IEnumerable<Order_detail> ? Order_Details { get; set; }
    }
}
