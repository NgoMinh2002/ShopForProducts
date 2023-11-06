
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopForProducts.Entities
{
    public class Account
    {
        [Key]
        public int UserId { get; set; }
        public string User_name { get; set; }
        [JsonIgnore]
        public string ?FullName { get; set; }
        [JsonIgnore]
        public string? Phone { get; set; }
        [JsonIgnore]
        public string? Email { get; set; }
        [JsonIgnore]
        public string? Address { get; set; }
        [JsonIgnore]
        public string? Avatar_url { get; set; }
        [JsonIgnore]
        public string PassWord { get; set; }
        [JsonIgnore]
        public int Status { get; set; }
        [JsonIgnore]
        public int? DecentralizationId { get; set; }

        [JsonIgnore]
        public Decentralization? decentralizations { get; set; }
        [JsonIgnore]
        public string? ResetPasswordToken { get; set; }
        [JsonIgnore]
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        [JsonIgnore]
        public DateTime ?created_at { get; set; }
        [JsonIgnore]
        public DateTime ?updated_at { get; set; }

        [JsonIgnore]
        public IEnumerable<Product_review>? product_Reviews { get; set; }
        [JsonIgnore]
        public IEnumerable<Order> ?orders { get; set; }
        [JsonIgnore]
        public IEnumerable<Carts> ?carts { get; set; }
       // public IEnumerable<RefreshToken>? refreshTokens { get; set; }
        
    }
}
