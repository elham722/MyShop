namespace MyShop.Contracts.DTOs.Statistics;

public class EventStoreStatistics
{
    public long TotalEvents { get; set; }
    
    public long TotalAggregates { get; set; }
    
    public long TotalSnapshots { get; set; }
    
    public double AverageEventsPerAggregate { get; set; }
    
    public DateTime? OldestEventTimestamp { get; set; }
    
    public DateTime? NewestEventTimestamp { get; set; }
    
    public long TotalSizeBytes { get; set; }
}