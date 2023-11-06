using ShopForProducts.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopForProducts.Model
{
    public class OrderWithStatusModel
    {
        public int OrderId { get; set; }
        public int? PaymentId { get; set; }
        public double? original_price { get; set; }
        public double? actual_price { get; set; }
        public string? full_name { get; set; }
        public string? Email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string status_name { get; set; }

       





    }
}
