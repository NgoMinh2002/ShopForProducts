
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Entities
{
    public class Decentralization
    {
        [Key]
        public int DecentralizationId { get; set; }
        public string Authority_name { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public IEnumerable<Account>? Accounts{ get; set; }
    }
}