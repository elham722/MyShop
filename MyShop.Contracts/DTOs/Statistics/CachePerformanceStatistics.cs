namespace MyShop.Contracts.DTOs.Statistics;

public class CachePerformanceStatistics
{
    public long CacheHits { get; set; }
    
    public long CacheMisses { get; set; }
    
    public double HitRatioPercentage => CacheHits + CacheMisses > 0 ? (double)CacheHits / (CacheHits + CacheMisses) * 100 : 0;
    
    public double AverageHitTimeMs { get; set; }
    
    public double AverageMissTimeMs { get; set; }
    
    public long TotalOperations => CacheHits + CacheMisses;
    
    public double EfficiencyScore => HitRatioPercentage * 0.7 + (100 - AverageHitTimeMs) * 0.3;
    
    public DateTime LastUpdateTime { get; set; }
    
    public long MemoryUsageBytes { get; set; }
    
    public double MemoryUsagePercentage { get; set; }
}