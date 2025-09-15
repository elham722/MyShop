using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.ExternalServices.Services;

namespace MyShop.ExternalServices.DependencyInjection;

public static class ExternalServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();

        return services;
    }
}
   
  

  