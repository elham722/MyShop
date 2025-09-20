using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop.Identity.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique identifier for the permission"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Human-readable name of the permission"),
                    Resource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The resource this permission applies to"),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The action this permission allows"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Detailed description of what this permission allows"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this permission was created"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this permission was last updated"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Who created this permission"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who last updated this permission"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether this permission is currently active"),
                    IsSystemPermission = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this is a system permission that cannot be modified"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Category for grouping permissions"),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 5, comment: "Priority level (1=highest, 10=lowest)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                },
                comment: "System permissions for fine-grained access control");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique identifier for the role"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Detailed description of what this role allows"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this role was created"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this role was last updated"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Who created this role"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who last updated this role"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether this role is currently active"),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this is a system role that cannot be modified"),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 5, comment: "Priority level for sorting (1=highest, 10=lowest)"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Category for grouping roles"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Human-readable name of the role"),
                    NormalizedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Normalized name for case-insensitive lookups"),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                },
                comment: "User roles for role-based access control");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique identifier for the user"),
                    Account_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When the account was created"),
                    Account_LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the user last logged in"),
                    Account_LastPasswordChangeAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the password was last changed"),
                    Account_LoginAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Number of login attempts"),
                    Account_IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether the account is active"),
                    Account_IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the account is deleted"),
                    Account_DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the account was deleted"),
                    Account_BranchId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Branch ID associated with the account"),
                    Security_TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether two-factor authentication is enabled"),
                    Security_TwoFactorSecret = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Two-factor authentication secret key"),
                    Security_TwoFactorEnabledAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When two-factor authentication was enabled"),
                    Security_Question = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Security question for account recovery"),
                    Security_Answer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Security answer for account recovery (encrypted)"),
                    Security_LastSecurityUpdate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the last security update was performed"),
                    Audit_CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who created the user"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When the user was created"),
                    Audit_ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who last modified the user"),
                    Audit_ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When the user was last modified"),
                    Audit_IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true, comment: "IP address of the last operation"),
                    Audit_UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "User agent of the last operation"),
                    CustomerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Foreign key to the customer entity"),
                    TotpSecretKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "TOTP secret key for two-factor authentication"),
                    TotpEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether TOTP is enabled"),
                    SmsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether SMS two-factor authentication is enabled"),
                    GoogleId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Google OAuth ID"),
                    MicrosoftId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Microsoft OAuth ID"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique username for the user"),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Normalized username for case-insensitive lookups"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Email address of the user"),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Normalized email for case-insensitive lookups"),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the email address has been confirmed"),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Hashed password of the user"),
                    SecurityStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Security stamp for token validation"),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Concurrency stamp for optimistic concurrency control"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "Phone number of the user"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the phone number has been confirmed"),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether two-factor authentication is enabled"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "When the lockout ends (null = not locked)"),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether lockout is enabled for this user"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Number of failed access attempts")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                },
                comment: "Application users with enhanced security and audit features");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "Identity",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Foreign key to the role"),
                    PermissionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Foreign key to the permission"),
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Unique identifier for the role-permission assignment"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this assignment was created"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this assignment was last updated"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Who created this assignment"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who last updated this assignment"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether this assignment is currently active"),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When this permission was assigned to the role"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "When this assignment expires (null = never expires)"),
                    AssignedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Who assigned this permission to the role"),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Whether this permission is granted (true) or denied (false)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission",
                        column: x => x.PermissionId,
                        principalSchema: "Identity",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Many-to-many relationship between roles and permissions");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Unique identifier for the audit log entry"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "ID of the user who performed the action"),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Action performed (e.g., Login, Logout, Create, Update, Delete)"),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Type of entity that was affected (e.g., User, Customer, Order)"),
                    EntityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "ID of the entity that was affected"),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "JSON representation of the old values before the change"),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "JSON representation of the new values after the change"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "When the action was performed"),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false, comment: "IP address of the client"),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "User agent string from the client"),
                    DeviceInfo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Device information (OS, Browser, etc.)"),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Session identifier"),
                    RequestId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Request correlation ID"),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Additional context data in JSON format"),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the operation was successful"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Error message if the operation failed"),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Severity level of the audit entry")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Action",
                schema: "Identity",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Entity_TypeId_Timestamp",
                schema: "Identity",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityId",
                schema: "Identity",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityType",
                schema: "Identity",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_IsSuccess",
                schema: "Identity",
                table: "AuditLogs",
                column: "IsSuccess");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Severity",
                schema: "Identity",
                table: "AuditLogs",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Severity_Timestamp",
                schema: "Identity",
                table: "AuditLogs",
                columns: new[] { "Severity", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId",
                schema: "Identity",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                schema: "Identity",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                schema: "Identity",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId_Timestamp",
                schema: "Identity",
                table: "AuditLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Category",
                schema: "Identity",
                table: "Permissions",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CreatedAt",
                schema: "Identity",
                table: "Permissions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_IsActive",
                schema: "Identity",
                table: "Permissions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_IsSystemPermission",
                schema: "Identity",
                table: "Permissions",
                column: "IsSystemPermission");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name_Unique",
                schema: "Identity",
                table: "Permissions",
                column: "Name",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Resource_Action_Unique",
                schema: "Identity",
                table: "Permissions",
                columns: new[] { "Resource", "Action" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_AssignedAt",
                schema: "Identity",
                table: "RolePermissions",
                column: "AssignedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_AssignedBy",
                schema: "Identity",
                table: "RolePermissions",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_ExpiresAt",
                schema: "Identity",
                table: "RolePermissions",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_IsActive",
                schema: "Identity",
                table: "RolePermissions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_IsGranted",
                schema: "Identity",
                table: "RolePermissions",
                column: "IsGranted");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Identity",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "Identity",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId_PermissionId_Unique",
                schema: "Identity",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Category",
                schema: "Identity",
                table: "Roles",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedAt",
                schema: "Identity",
                table: "Roles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsActive",
                schema: "Identity",
                table: "Roles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsSystemRole",
                schema: "Identity",
                table: "Roles",
                column: "IsSystemRole");

            migrationBuilder.CreateIndex(
                name: "IX_Role_NormalizedName_Unique",
                schema: "Identity",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Priority",
                schema: "Identity",
                table: "Roles",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Category_Priority",
                schema: "Identity",
                table: "Roles",
                columns: new[] { "Category", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name_Unique",
                schema: "Identity",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_AccessFailedCount",
                schema: "Identity",
                table: "Users",
                column: "AccessFailedCount");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Account_BranchId",
                schema: "Identity",
                table: "Users",
                column: "Account_BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Account_CreatedAt",
                schema: "Identity",
                table: "Users",
                column: "Account_CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Account_IsActive",
                schema: "Identity",
                table: "Users",
                column: "Account_IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Account_IsDeleted",
                schema: "Identity",
                table: "Users",
                column: "Account_IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Account_LastLoginAt",
                schema: "Identity",
                table: "Users",
                column: "Account_LastLoginAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Audit_CreatedAt",
                schema: "Identity",
                table: "Users",
                column: "Audit_CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Audit_CreatedBy",
                schema: "Identity",
                table: "Users",
                column: "Audit_CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Audit_IpAddress",
                schema: "Identity",
                table: "Users",
                column: "Audit_IpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Audit_ModifiedAt",
                schema: "Identity",
                table: "Users",
                column: "Audit_ModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Audit_ModifiedBy",
                schema: "Identity",
                table: "Users",
                column: "Audit_ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_CustomerId",
                schema: "Identity",
                table: "Users",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_CustomerId_EmailConfirmed",
                schema: "Identity",
                table: "Users",
                columns: new[] { "CustomerId", "EmailConfirmed" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email_Unique",
                schema: "Identity",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_EmailConfirmed",
                schema: "Identity",
                table: "Users",
                column: "EmailConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_GoogleId",
                schema: "Identity",
                table: "Users",
                column: "GoogleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_LockoutEnabled_LockoutEnd",
                schema: "Identity",
                table: "Users",
                columns: new[] { "LockoutEnabled", "LockoutEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_LockoutEnd",
                schema: "Identity",
                table: "Users",
                column: "LockoutEnd");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_MicrosoftId",
                schema: "Identity",
                table: "Users",
                column: "MicrosoftId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_NormalizedEmail_Unique",
                schema: "Identity",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_NormalizedUserName_Unique",
                schema: "Identity",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_PhoneNumberConfirmed",
                schema: "Identity",
                table: "Users",
                column: "PhoneNumberConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Security_LastSecurityUpdate",
                schema: "Identity",
                table: "Users",
                column: "Security_LastSecurityUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Security_TwoFactorEnabled",
                schema: "Identity",
                table: "Users",
                column: "Security_TwoFactorEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Security_TwoFactorEnabledAt",
                schema: "Identity",
                table: "Users",
                column: "Security_TwoFactorEnabledAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_SmsEnabled",
                schema: "Identity",
                table: "Users",
                column: "SmsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_TotpEnabled",
                schema: "Identity",
                table: "Users",
                column: "TotpEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_TotpEnabled_SmsEnabled",
                schema: "Identity",
                table: "Users",
                columns: new[] { "TotpEnabled", "SmsEnabled" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_TwoFactorEnabled",
                schema: "Identity",
                table: "Users",
                column: "TwoFactorEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_UserName_Unique",
                schema: "Identity",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");
        }
    }
}
