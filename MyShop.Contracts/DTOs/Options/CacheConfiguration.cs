namespace MyShop.Contracts.DTOs.Options;

public class CacheConfiguration
{
    public string ConnectionString { get; set; } = "localhost:6379";
    
    public int DatabaseNumber { get; set; } = 0;
  
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
    
    public string KeyPrefix { get; set; } = "DigiTekShop";
    
    public bool EnableCompression { get; set; } = true;
    
    public bool EnableSerializationOptimization { get; set; } = true;
    
    public long MaxCacheSizeBytes { get; set; } = 100 * 1024 * 1024; // 100MB
    
    public CacheEvictionPolicy EvictionPolicy { get; set; } = CacheEvictionPolicy.LRU;
    
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(30);
    
    public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(10);
    
    public int MaxRetryCount { get; set; } = 3;
    
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500);
    
    public bool EnableStatistics { get; set; } = true;
    
    public TimeSpan StatisticsResetInterval { get; set; } = TimeSpan.FromHours(1);
}