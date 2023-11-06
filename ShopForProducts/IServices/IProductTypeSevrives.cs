using Microsoft.Identity.Client;
using ShopForProducts.Entities;

namespace ShopForProducts.IServices
{
    public interface IProductTypeSevrives
    {
        Task<Product_type> Addproducttype(string ProductType_name);
        Task<Product_type> Updateproducttype(int ProducttypeId, string productType_name);
        Task<Product_type> Deleteproducttype(string productType_name);
        Task<List<Product_type>> Showproducttype();
    }
}
