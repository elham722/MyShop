namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Query parameters for getting user tokens
    /// </summary>
    public class GetUserTokensQueryParams
    {
        public string? UserId { get; set; }
        public string? LoginProvider { get; set; }
        public string? TokenType { get; set; }
        public string? TokenPurpose { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsRevoked { get; set; }
        public bool? IsExpired { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? ExpiresAfter { get; set; }
        public DateTime? ExpiresBefore { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortDirection { get; set; } = "desc";
    }
}