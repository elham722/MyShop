using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Identity.Authorization.Handlers;
using MyShop.Identity.Constants;
using MyShop.Identity.Context;
using MyShop.Identity.Models;
using MyShop.Identity.Mappings;
using MyShop.Contracts.Identity.Services.Audit;
using MyShop.Contracts.Identity.Services.Authentication;
using MyShop.Contracts.Identity.Services;
using MyShop.Contracts.Identity.Services.JwtToken;
using MyShop.Contracts.Identity.Services.RolePermission;
using MyShop.Contracts.DTOs.Options;
using MyShop.Identity.Services;
using MyShop.Identity.Services.Audit;
using MyShop.Identity.Services.Authentication;
using MyShop.Identity.Services.Authorization;
using MyShop.Identity.Services.JwtToken;
using MyShop.Identity.Services.RolePermission;

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
                options.DefaultChallengeScheme = Constants.IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = Constants.IdentityConstants.ApplicationScheme;
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
            
            // Configure Mapster mappings
            services.AddMapster();

            // Register individual authentication services
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITwoFactorService, TwoFactorService>();
            services.AddScoped<ILockoutService, LockoutService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Register authentication facade
            services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();

            // Register other identity services
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<Contracts.Identity.Services.Authorization.IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            
            // Configure options
            services.Configure<LockoutOptions>(configuration.GetSection("LockoutOptions"));
            
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
