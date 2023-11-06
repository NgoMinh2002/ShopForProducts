using ShopForProducts.Entities;
using ShopForProducts.Model;
namespace ShopForProducts.IServices
{
    public interface IProductreviewsServices
    {
        Task<bool> CanSubmitProductReview(int ?userId, int? productId);
        Task<Product_review> SubmitProductReview(int userId, ProductreviewsModel modle);
        Task<Product_review> Updateproduct_Review(int Product_reviewId, updateView modle);
        Task<Product_review> DeteleReview(int Product_reviewId);
        Task<List<Product_review>> Get_Review();
    }
}
