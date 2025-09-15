namespace MyShop.Contracts.DTOs.Identity.CacheService;

public class CacheStatistics
{
    public long CacheHits { get; set; }
    
    public long CacheMisses { get; set; }
    
    public double HitRatio => CacheHits + CacheMisses > 0 ? (double)CacheHits / (CacheHits + CacheMisses) * 100 : 0;
    
    public long TotalCachedItems { get; set; }
    
    public long TotalCacheSizeBytes { get; set; }
    
    public double AverageItemSizeBytes => TotalCachedItems > 0 ? (double)TotalCacheSizeBytes / TotalCachedItems : 0;
    
    public long CacheEvictions { get; set; }
    
    public double MemoryUsagePercentage { get; set; }
    
    public DateTime LastResetTime { get; set; }
}