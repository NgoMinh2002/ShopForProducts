using ShopForProducts.Entities;
using ShopForProducts.Model;

namespace ShopForProducts.IServices
{
    public interface IProductSevrives
    {
        Task<Product> AddProduct(ProductModel  model);
        Task<Product> GetProduct(string nameproduct);
        Task<Product> UpdateProduct(string nameproduct, Updateprpcuct productupdate);
        Task<List<Product>> Showproduct();
        Task<List<Product>> GetRelatedProducts(string nameproduct, int maxRelatedProducts = 5);
        Task<List<Product>> GetOutstandingproduct(int count);
        Task<Product> DeteleProducts(string nameproduct);
        Task<int> GetProductViews(int productId);




    }
}

