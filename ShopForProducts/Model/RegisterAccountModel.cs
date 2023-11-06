using System.ComponentModel.DataAnnotations;

namespace ShopForProducts.Model
{
    public class RegisterAccountModel
    {

        [Required]
        public string User_name { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string PassWord { get; set;}
        [Required]
        public string Confirmpassword { get; set;}
    }
}
