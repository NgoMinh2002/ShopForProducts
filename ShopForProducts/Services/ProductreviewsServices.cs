using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;

namespace ShopForProducts.Services
{
    public class ProductreviewsServices : IProductreviewsServices
    {
        private readonly AppDbcontext appDbcontext;
        public ProductreviewsServices()
        {
            appDbcontext = new AppDbcontext();
        }

        public async Task<bool> CanSubmitProductReview(int? userId, int? productId)
        {
            var query = from orderDetail in appDbcontext.dboOrder_detail
                        join order in appDbcontext.dboOrders on orderDetail.OrderId equals order.OrderId
                        join orderStatus in appDbcontext.dboOrder_status on order.Order_statusId equals orderStatus.Order_statusId
                        where order.UserId == userId && orderDetail.ProductId == productId
                        select new
                        {
                            OrderDetail = orderDetail,
                            Order = order,
                            OrderStatus = orderStatus
                        };

            var orderInfo = query.AsEnumerable().FirstOrDefault(o => string.Equals(o.OrderStatus.status_name, "Đã Hoàn Thành", StringComparison.OrdinalIgnoreCase));

            return orderInfo != null;
        }

        public async Task<Product_review> DeteleReview(int Product_reviewId)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var review = await appDbcontext.dboProduct_Reviews.FirstOrDefaultAsync(v => v.Product_reviewId == Product_reviewId);
                    if (review == null)
                    {
                        throw new Exception("không tìm thấy để xóa");
                    }
                    else
                    {
                        appDbcontext.dboProduct_Reviews.Remove(review);
                        await appDbcontext.SaveChangesAsync();
                        transaction.Commit();
                        return review;
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<Product_review>> Get_Review()
        {
            List<Product_review> productReviews = await appDbcontext.dboProduct_Reviews.ToListAsync();
            if (productReviews == null)
            {
                throw new Exception("Danh sách trống");

            }
            else
            {
                return productReviews;
            }  
        }
        public async Task<Product_review> SubmitProductReview(int userId, ProductreviewsModel modle)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {

                try
                {

                    if (!await CanSubmitProductReview(userId, modle.ProductId))
                    {

                        throw new Exception("Không thể đánh giá sản phẩm");
                    }
                    else
                    {
                        var productReview = new Product_review()
                        {   UserId = userId,
                            ProductId = modle.ProductId,
                            content_rated = modle.content_rated,
                            point_evaluation = modle.point_evaluation,
                            content_seen = modle.content_seen,
                            status = 1,
                            created_at = DateTime.Now,
                        };
                        appDbcontext.dboProduct_Reviews.Add(productReview);
                        await appDbcontext.SaveChangesAsync();
                        transaction.Commit();
                        return productReview;

                    }


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }


        }
        public async Task<Product_review> Updateproduct_Review(int Product_reviewId, updateView modle)
        {
            using (var transaction = await appDbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var review = await appDbcontext.dboProduct_Reviews.FirstOrDefaultAsync(v => v.Product_reviewId == Product_reviewId);
                    if (review == null)
                    {
                        throw new Exception("Không có đánh giá trong danh sách");
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(modle.content_seen))
                        {
                            review.content_seen = modle.content_seen;
                        }
                        if (!string.IsNullOrEmpty(modle.content_rated))
                        {
                            review.content_rated = modle.content_rated;
                        }
                        if (modle.point_evaluation > 0)
                        {
                            review.point_evaluation = modle.point_evaluation;
                        }
                        if (modle.status >= 0)
                        {
                            review.status = modle.status;
                        }
                        appDbcontext.dboProduct_Reviews.Update(review);
                        await appDbcontext.SaveChangesAsync();
                        transaction.Commit();
                        return review;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
