


using System.Text.Json.Serialization;

namespace ShopForProducts.Model
{

    public class Updateprpcuct
    {
       

        public int? Product_typeId { get; set; }
        [JsonIgnore] // Khóa ngoại để liên kết với loại sản phẩm.   
        public double? price { get; set; }
        [JsonIgnore]
        public string? avartar_image_product { get; set; } // URL hoặc đường dẫn đến hình ảnh đại diện của sản phẩm.
        [JsonIgnore]
        public string? title { get; set; } // Mô tả hoặc tiêu đề của sản phẩm.
        [JsonIgnore]
        public int? discount { get; set; } // Giảm giá được áp dụng cho sản phẩm.
        [JsonIgnore]
        public int? status { get; set; } // Trạng thái của sản phẩm (sẵn sàng, hết hàng, v.v.).
        [JsonIgnore]
        public int? number_of_views { get; set; } // Số lượng lượt xem hoặc truy cập của sản phẩm.
       
         
        
    }
}
