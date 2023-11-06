using Microsoft.EntityFrameworkCore;
using ShopForProducts.Entities;
using ShopForProducts.IServices;

namespace ShopForProducts.Services
{
    public class ProductTypeSevrives : IProductTypeSevrives
    {
        private readonly AppDbcontext _dbcontext;
        public ProductTypeSevrives()
        {
            _dbcontext = new AppDbcontext();
        }
        public async Task<Product_type> Addproducttype(string ProductType_name)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var producttype = await _dbcontext.dboProduct_Types.FirstOrDefaultAsync(p => p.name_product_type == ProductType_name);

                    if (producttype != null)
                    {
                        throw new Exception(" Sản phẩm này đã có.");
                    }
                    else
                    {
                        var Producttype = new Product_type
                        {
                            name_product_type = ProductType_name,
                            created_at = DateTime.Now
                        };
                        _dbcontext.dboProduct_Types.Add(Producttype);
                        await _dbcontext.SaveChangesAsync();
                        transection.Commit();
                        return Producttype;

                    }
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Thêm không thành công" + ex.Message);
                }
            }
        }
        public async Task<Product_type> Deleteproducttype(string productType_name)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var producttype = await _dbcontext.dboProduct_Types.FirstOrDefaultAsync(p => p.name_product_type == productType_name);
                    if (producttype == null)
                    {
                        throw new Exception("Loại sản phẩm không tồn tại");
                    }
                    else
                    {
                        var resetProuduct = await _dbcontext.dboProducts.Where(p => p.Product_typeId == producttype.Product_typeId).ToListAsync();
                        foreach (var product in resetProuduct)
                        {
                            product.Product_typeId = null;
                        }
                        _dbcontext.dboProduct_Types.Remove(producttype);
                        await _dbcontext.SaveChangesAsync();
                        transection.Commit();
                    }
                    return producttype;

                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Xóa thất bại" + ex.Message);
                }
            }
        }
        public async Task<List<Product_type>> Showproducttype()
        {
            var producttype = await _dbcontext.dboProduct_Types.ToListAsync();
            if (producttype == null)
            {
                throw new Exception("Danh sách trống");
            }
            else
            {
                return producttype;
            }

        }

        public async Task<Product_type> Updateproducttype(int ProducttypeId, string productType_name)
        {
            using (var transection = await _dbcontext.Database.BeginTransactionAsync())
            {
                try
                {
                    var update = await _dbcontext.dboProduct_Types.FirstOrDefaultAsync(p => p.Product_typeId == ProducttypeId);
                    if (update == null)
                    {
                        throw new Exception("Loại sản phẩm không có");

                    }
                    else
                    {
                        update.name_product_type = productType_name;
                        update.updated_at = DateTime.Now;
                        _dbcontext.dboProduct_Types.Update(update);
                        await _dbcontext.SaveChangesAsync();
                        transection.Commit();
                    }
                    return update;
                }
                catch (Exception ex)
                {
                    transection.Rollback();
                    throw new Exception("Cập nhật thất bại! " + ex.Message);
                }
            }
        }
    }
}
