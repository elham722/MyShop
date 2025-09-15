namespace MyShop.Identity.Constants
{
    /// <summary>
    /// Constants for Identity configuration and business rules
    /// </summary>
    public static class IdentityConstants
    {
        /// <summary>
        /// Authentication scheme name
        /// </summary>
        public const string ApplicationScheme = "MyShopIdentity";
        /// <summary>
        /// Password expiration policy
        /// </summary>
        public static class PasswordPolicy
        {
            public const int ExpirationDays = 90;
            public const int MinimumLength = 8;
            public const int RequiredUniqueChars = 1;
        }

        /// <summary>
        /// Account lockout policy
        /// </summary>
        public static class LockoutPolicy
        {
            public const int MaxFailedAttempts = 5;
            public const int LockoutDurationMinutes = 5;
        }

        /// <summary>
        /// Role priority levels (lower number = higher priority)
        /// </summary>
        public static class RolePriority
        {
            public const int SuperAdmin = 1;
            public const int SystemAdmin = 2;
            public const int Admin = 3;
            public const int Manager = 4;
            public const int CustomerService = 5;
            public const int SalesRep = 6;
            public const int SupportAgent = 7;
            public const int Auditor = 5;
            public const int ReportViewer = 6;
            public const int Customer = 8;
            public const int Guest = 9;
        }

        /// <summary>
        /// Session and token policies
        /// </summary>
        public static class SessionPolicy
        {
            public const int SessionTimeoutMinutes = 30;
            public const int RefreshTokenExpirationDays = 7;
        }
    }
}