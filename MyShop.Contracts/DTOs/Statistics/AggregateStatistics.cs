namespace MyShop.Contracts.DTOs.Statistics;

public class AggregateStatistics
{
    public Guid AggregateId { get; set; }
    
    public int CurrentVersion { get; set; }
    
    public long TotalEvents { get; set; }
    
    public int SnapshotCount { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? LastModifiedAt { get; set; }
    
    public long TotalSizeBytes { get; set; }
}