using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Identity.Context;

namespace MyShop.Identity.DependencyInjection
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MyShopIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDBConnection")));

            return services;
        }

    }
}
