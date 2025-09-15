using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Contracts.Identity.Services;
using MyShop.Identity.Constants;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Services;
using IAuthorizationService = MyShop.Identity.Services.IAuthorizationService;

namespace MyShop.Identity.DependencyInjection
{
    public static class IdentityServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure DbContext
            services.AddDbContext<MyShopIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDBConnection")));

            // Configure Identity Services
            services.AddIdentity<ApplicationUser, Role>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false; // Set to true in production
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<MyShopIdentityDbContext>()
            .AddDefaultTokenProviders();

            // Configure Identity Options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false; // Set to true in production
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            // Configure Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Constants.IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = Constants.IdentityConstantss.ApplicationScheme;
                options.DefaultSignInScheme = Constants.IdentityConstantss.ApplicationScheme;
            });

            // Configure Authorization
            services.AddAuthorization(options =>
            {
                // Add default policy
                options.AddPolicy("RequireAuthenticatedUser", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

                // Add role-based policies
                options.AddPolicy("SuperAdminOnly", policy =>
                {
                    policy.RequireRole(RoleConstants.System.SuperAdmin);
                });

                options.AddPolicy("AdminOrAbove", policy =>
                {
                    policy.RequireRole(
                        RoleConstants.System.SuperAdmin,
                        RoleConstants.System.SystemAdmin,
                        RoleConstants.Administrative.Admin
                    );
                });

                options.AddPolicy("ManagerOrAbove", policy =>
                {
                    policy.RequireRole(
                        RoleConstants.System.SuperAdmin,
                        RoleConstants.System.SystemAdmin,
                        RoleConstants.Administrative.Admin,
                        RoleConstants.Administrative.Manager
                    );
                });

                options.AddPolicy("BusinessUserOrAbove", policy =>
                {
                    policy.RequireRole(
                        RoleConstants.System.SuperAdmin,
                        RoleConstants.System.SystemAdmin,
                        RoleConstants.Administrative.Admin,
                        RoleConstants.Administrative.Manager,
                        RoleConstants.Business.CustomerService,
                        RoleConstants.Business.SalesRep,
                        RoleConstants.Business.SupportAgent
                    );
                });
            });

            // Register custom services
            services.AddScoped<RoleManager<Role>>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            
            // Register custom identity services
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IIdentityConfigurationService, IdentityConfigurationService>();
            services.AddScoped<IIdentityStatisticsService, IdentityStatisticsService>();
            services.AddScoped<IIdentityNotificationService, IdentityNotificationService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IIdentityHealthCheckService, IdentityHealthCheckService>();
            services.AddScoped<IIdentityReportService, IdentityReportService>();
            services.AddScoped<IIdentityCacheService, IdentityCacheService>();
            services.AddScoped<IIdentityValidationService, IdentityValidationService>();
            
            // Register cached services
            services.AddScoped<CachedUserService>();
            services.AddScoped<CachedRoleService>();
            services.AddScoped<CachedPermissionService>();
            
            // Register authorization handlers
            services.AddScoped<IAuthorizationHandler, ResourceAuthorizationHandler>();

            return services;
        }

        /// <summary>
        /// Seeds the database with initial data
        /// </summary>
        public static async Task SeedIdentityDataAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyShopIdentityDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed data
            await IdentitySeedData.SeedAsync(context, roleManager, userManager);
        }
    }
}
