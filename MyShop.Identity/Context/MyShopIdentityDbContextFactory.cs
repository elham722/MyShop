using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyShop.Identity.Context;

namespace MyShop.Identity.Context
{
    public class MyShopIdentityDbContextFactory : IDesignTimeDbContextFactory<MyShopIdentityDbContext>
    {
        public MyShopIdentityDbContext CreateDbContext(string[] args)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<MyShopIdentityDbContext>();

                // مستقیم Connection String داخل Factory
                optionsBuilder.UseSqlServer(
                    "Data Source=.;Initial catalog=MyShopIdentity_DB;Integrated security=true;TrustServerCertificate=True;");

                var context = new MyShopIdentityDbContext(optionsBuilder.Options);
                
                // Test if context can be created successfully
                return context;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create DbContext: {ex.Message}", ex);
            }
        }
    }
}