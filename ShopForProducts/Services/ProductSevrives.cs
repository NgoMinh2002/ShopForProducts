using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;
using ShopForProducts.Model;

namespace ShopForProducts.Services

{
    public class ProductSevrives : IProductSevrives
    {
        private readonly AppDbcontext _dbcontext;
        public ProductSevrives()
        {

            _dbcontext = new AppDbcontext();
        }
        public async Task<Product> AddProduct(ProductModel model)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _dbcontext.dboProducts.FirstOrDefaultAsync(p => p.name_product == model.name_product);
                    if (product != null)
                    {
                        throw new Exception($"Sản phẩm {model.name_product} đã có trong danh sách");
                    }
                    else
                    {

                        var producttype = await _dbcontext.dboProduct_Types.FirstOrDefaultAsync(p => p.Product_typeId == model.Product_typeId);
                        if (producttype != null)
                        {

                            var data = new Product
                            {
                                Product_typeId = model.Product_typeId,
                                name_product = model.name_product,
                                price = model.price,
                                avartar_image_product = model.avartar_image_product,
                                title = model.title,
                                status = 1,
                                discount = model.discount,
                                number_of_views = 0,
                                created_at = DateTime.Now,
                            };
                            _dbcontext.dboProducts.Add(data);
                            await _dbcontext.SaveChangesAsync();
                            transection.Commit();
                            return data;

                        }
                        else
                        {
                            throw new Exception("Thêm thất bại! Không có loại sản phẩm!");
                        }


                    }

                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Lỗi không thể thêm!" + ex.Message);
                }
            }
        }

        public async Task<Product> DeteleProducts(string nameproduct)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    string namproductlower = nameproduct.ToLower();
                    var product = await _dbcontext.dboProducts.FirstOrDefaultAsync(p => p.name_product.ToLower() == namproductlower);
                    if (namproductlower == null)
                    {
                        throw new Exception($"Không có sản phẩm {product.name_product}  trong danh sách");
                    }
                    else
                    {
                        var resetCartitem = await _dbcontext.dboCart_item.Where(p => p.ProductId == product.ProductId).ToListAsync();
                        foreach (var caritem in resetCartitem)
                        {
                            caritem.ProductId = null;
                        }
                        var restorderdetail = await _dbcontext.dboOrder_detail.Where(p => p.ProductId == product.ProductId).ToListAsync();
                        foreach (var orderdetail in restorderdetail)
                        {
                            orderdetail.ProductId = null;
                        }
                        var resproductreview = await _dbcontext.dboProduct_Reviews.Where(p => p.ProductId == product.ProductId).ToListAsync();
                        foreach (var view in resproductreview)
                        {
                            view.ProductId = null;
                        }
                        _dbcontext.dboProducts.Remove(product);
                        await _dbcontext.SaveChangesAsync();
                        transection.Commit();
                        return product;
                    }
                }catch(Exception exx)
                {
                    throw new Exception(exx.Message);
                }
            }
        }

        public async Task<List<Product>> GetOutstandingproduct(int count)
        {
            var Outstandingproduct = _dbcontext.dboProducts.OrderByDescending(p => p.number_of_views)
            .Take(count)
            .ToList();
            return Outstandingproduct;
        }

        public async Task<Product> GetProduct(string nameproduct)
        {
            try
            {
                var product = await _dbcontext.dboProducts.FirstOrDefaultAsync(p => p.name_product == nameproduct);
                if (product == null)
                {
                    throw new Exception("Không có sản phẩm trong danh sách ");
                }
                else
                {
                    product.number_of_views++;
                    _dbcontext.dboProducts.Update(product);
                    await _dbcontext.SaveChangesAsync();
                    return product;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi: " + ex.Message);
            }
        }

        public async Task<int> GetProductViews(int productId)
        {
            try
            {
                var product = _dbcontext.dboProducts.Find(productId);
                if (product != null)
                {
                    string name = product.name_product;
                    int number_of_views = (int)product.number_of_views;
                    throw new Exception($"sản phẩm có tên là ( {name} ) có số lượt  xem là {number_of_views}");
                }
                else
                {
                    throw new Exception("Không tìm thấy sản phẩm");
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            
            
        }

        /* public async Task<List<Product>> GetRelatedProducts(string nameproduct, int maxRelatedProducts)
         {
             using (var transection = await _dbcontext.Database.BeginTransactionAsync())
             {
                 var product = await _dbcontext.dboProducts
                        .Include(p => p.product_Type)
                        .FirstOrDefaultAsync(p => p.name_product == nameproduct);
                 *//*var product = await _dbcontext.dboProducts.FirstOrDefaultAsync(p => p.name_product == nameproduct);*//*
                 if (product == null)
                 {
                     throw new Exception("Không có sản phẩm trong danh sách ");
                 }
                 else
                 {
                     var relatedProducts = await _dbcontext.dboProducts
                     .Where(p => p.name_product != nameproduct && p.Product_typeId == product.Product_typeId)
                     .Take(maxRelatedProducts)
                     .ToListAsync();

                     return relatedProducts;
                 }


             }
         }*/
        public async Task<List<Product>> GetRelatedProducts(string nameproduct, int maxRelatedProducts)
        {
            var product = await GetProduct(nameproduct);

            if (product == null)
            {
                return new List<Product>(); // Trả về danh sách trống nếu không tìm thấy sản phẩm gốc
            }

            var relatedProducts = await _dbcontext.dboProducts
                .Where(p => p.name_product != nameproduct && p.Product_typeId == product.Product_typeId)
                .Take(maxRelatedProducts)
                .ToListAsync();

            return relatedProducts;
        }
        /*public async Task<List<Product>> GetRelatedProducts(string nameproduct, int maxRelatedProducts)
        {
            try
            {
                var originalProduct = await _dbcontext.dboProducts
                    .Include(p => p.product_Type)
                    .FirstOrDefaultAsync(p => p.name_product == nameproduct);
                if (originalProduct == null)
                {
                    return new List<Product>();
                }
                else
                {
                     
                     var relatedProducts = await _dbcontext.dboProducts
                  
                    .Where(p => p.name_product != nameproduct && p.product_Type.Product_typeId == originalProduct.product_Type.Product_typeId)
                    .Take(maxRelatedProducts)
                    .ToListAsync();

                    return relatedProducts;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
*/
        public async Task<List<Product>> Showproduct()
        {
            var product = await _dbcontext.dboProducts.ToListAsync();
            if (product == null)
            {
                throw new Exception("Danh sách trống");

            }
            else
            {
                return product;
            }
        }

        public async Task<Product> UpdateProduct(string nameproduct, Updateprpcuct productupdate)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _dbcontext.dboProducts.FirstOrDefaultAsync(p => p.name_product == nameproduct);
                    if (product == null)
                    {
                        throw new Exception("Không có sản phẩm trong danh sách ");
                    }
                    else
                    {
                        var producttype = await _dbcontext.dboProduct_Types.FirstOrDefaultAsync(p => p.Product_typeId == productupdate.Product_typeId);
                        if (producttype == null)
                        {
                            throw new Exception("Loại sản phẩm không tồn tại ");
                        }
                        else
                        {
                            if (productupdate.Product_typeId > 0)
                            {
                                product.Product_typeId = productupdate.Product_typeId;
                            }
                            if (productupdate.price > 0)
                            {
                                product.price = productupdate.price;
                            }

                            if (productupdate.discount >= 0)
                            {
                                product.discount = productupdate.discount;
                            }

                            if (!string.IsNullOrEmpty(productupdate.avartar_image_product))
                            {
                                product.avartar_image_product = productupdate.avartar_image_product;
                            }

                            if (!string.IsNullOrEmpty(productupdate.title))
                            {
                                product.title = productupdate.title;
                            }
                            if (productupdate.status > 0)
                            {
                                product.status = productupdate.status;
                            }
                            if (productupdate.number_of_views > 0)
                            {
                                product.number_of_views = productupdate.number_of_views;
                            }
                            product.updated_at = DateTime.Now;
                            _dbcontext.dboProducts.Update(product);
                            await _dbcontext.SaveChangesAsync();
                            transection.Commit();
                            return product;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Lỗi không thể cập nhật! " + ex.Message);
                }
            }
        }
    }
}


