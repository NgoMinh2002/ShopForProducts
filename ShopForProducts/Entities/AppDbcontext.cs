
using Microsoft.EntityFrameworkCore;


namespace ShopForProducts.Entities
{
    public class AppDbcontext : DbContext
    {

        public DbSet<VnPay> vnPays { get; set; }
        public DbSet<Account> dboAccounts { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }

        public DbSet<Product> dboProducts { get; set; }
        public DbSet<Carts> dboCrats { get; set; }
        public DbSet<Decentralization> dboDecentralizations { get; set; }
        public DbSet<Order> dboOrders { get; set; }
        public DbSet<Cart_item> dboCart_item { get; set; }
        public DbSet<Order_detail> dboOrder_detail { get; set; }
        public DbSet<Order_status> dboOrder_status { get; set; }
        public DbSet<Payment> dboPayment { get; set; }
        public DbSet<Product_type> dboProduct_Types { get; set; }
        public DbSet<Product_review> dboProduct_Reviews { get; set; }
        /* public AppDbcontext(DbContextOptions<AppDbcontext>options): base(options)
         {

         }*/
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-D6GDG00; Database = ShopPolyFood; Trusted_Connection = True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);

        }

    }
}





