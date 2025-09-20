using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyShop.Identity.Context;

namespace MyShop.Identity.Context
{
    public class MyShopIdentityDbContextFactory : IDesignTimeDbContextFactory<MyShopIdentityDbContext>
    {
        public MyShopIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyShopIdentityDbContext>();

            // مستقیم Connection String داخل Factory
            optionsBuilder.UseSqlServer(
                "Data Source=.;Initial catalog=BackendIdentity_DB;Integrated security=true;TrustServerCertificate=True;");

            return new MyShopIdentityDbContext(optionsBuilder.Options);
        }
    }
}