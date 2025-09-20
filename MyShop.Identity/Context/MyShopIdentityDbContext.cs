using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Models;
using MyShop.Identity.Configurations;

namespace MyShop.Identity.Context
{
    /// <summary>
    /// Identity DbContext for MyShop application with custom entities and configurations
    /// </summary>
    public class MyShopIdentityDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        public MyShopIdentityDbContext(DbContextOptions<MyShopIdentityDbContext> options) : base(options)
        {
        }

        // Custom DbSets
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                base.OnModelCreating(builder);

                // Apply configurations one by one to identify the problematic one
                try
                {
                    builder.ApplyConfiguration(new ApplicationUserConfiguration());
                    Console.WriteLine("ApplicationUserConfiguration applied successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in ApplicationUserConfiguration: {ex.Message}");
                    throw;
                }

                try
                {
                    builder.ApplyConfiguration(new RoleConfiguration());
                    Console.WriteLine("RoleConfiguration applied successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in RoleConfiguration: {ex.Message}");
                    throw;
                }

                try
                {
                    builder.ApplyConfiguration(new PermissionConfiguration());
                    Console.WriteLine("PermissionConfiguration applied successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in PermissionConfiguration: {ex.Message}");
                    throw;
                }

                // UserRoleConfiguration removed - using default IdentityUserRole<string>

                try
                {
                    builder.ApplyConfiguration(new RolePermissionConfiguration());
                    Console.WriteLine("RolePermissionConfiguration applied successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in RolePermissionConfiguration: {ex.Message}");
                    throw;
                }

                // Using default Identity classes - custom configurations removed

                try
                {
                    builder.ApplyConfiguration(new AuditLogConfiguration());
                    Console.WriteLine("AuditLogConfiguration applied successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in AuditLogConfiguration: {ex.Message}");
                    throw;
                }

                // Configure table names and schema
                ConfigureTableNames(builder);
                
                // Configure relationships
                ConfigureRelationships(builder);
                
                // Configure indexes
                ConfigureIndexes(builder);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error in OnModelCreating: {ex.Message}", ex);
            }
        }

        private void ConfigureTableNames(ModelBuilder builder)
        {
            // Set schema for all tables
            builder.Entity<ApplicationUser>().ToTable("Users", "Identity");
            builder.Entity<Role>().ToTable("Roles", "Identity");
            builder.Entity<Permission>().ToTable("Permissions", "Identity");
            builder.Entity<RolePermission>().ToTable("RolePermissions", "Identity");
            builder.Entity<AuditLog>().ToTable("AuditLogs", "Identity");
            
            // Default Identity tables will use default names and schema
        }

        private void ConfigureRelationships(ModelBuilder builder)
        {
            // Configure RolePermission with composite key
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            // Role -> RolePermissions (One-to-Many)
            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Permission -> RolePermissions (One-to-Many)
            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> AuditLogs (One-to-Many)
            builder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Keep audit logs even if user is deleted
        }

        private void ConfigureIndexes(ModelBuilder builder)
        {
            // Audit log indexes for better performance
            builder.Entity<AuditLog>()
                .HasIndex(al => new { al.UserId, al.Timestamp })
                .HasDatabaseName("IX_AuditLogs_UserId_Timestamp");

            builder.Entity<AuditLog>()
                .HasIndex(al => new { al.EntityType, al.EntityId })
                .HasDatabaseName("IX_AuditLogs_EntityType_EntityId");

            builder.Entity<AuditLog>()
                .HasIndex(al => al.Timestamp)
                .HasDatabaseName("IX_AuditLogs_Timestamp");

            // Role indexes for performance
            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("IX_Roles_Name_Unique");

            builder.Entity<Role>()
                .HasIndex(r => new { r.Category, r.Priority })
                .HasDatabaseName("IX_Roles_Category_Priority");

            // Permission indexes for performance
            builder.Entity<Permission>()
                .HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("IX_Permissions_Name_Unique");

            builder.Entity<Permission>()
                .HasIndex(p => new { p.Resource, p.Action })
                .IsUnique()
                .HasDatabaseName("IX_Permissions_Resource_Action_Unique");
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    // Set CreatedAt for entities that have this property
                    if (entity.GetType().GetProperty("CreatedAt") != null)
                    {
                        entry.Property("CreatedAt").CurrentValue = now;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    // Set UpdatedAt for entities that have this property
                    if (entity.GetType().GetProperty("UpdatedAt") != null)
                    {
                        entry.Property("UpdatedAt").CurrentValue = now;
                    }
                }
            }
        }
    }
}
