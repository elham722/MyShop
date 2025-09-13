namespace MyShop.Contracts.DTOs.Options;

public class BulkOperationProgress
{
    public int CurrentBatch { get; set; }
    
    public int TotalBatches { get; set; }
    
    public int ProcessedCount { get; set; }
    
    public int TotalCount { get; set; }
    
    public double ProgressPercentage => TotalCount > 0 ? (double)ProcessedCount / TotalCount * 100 : 0;
    
    public TimeSpan? EstimatedTimeRemaining { get; set; }
}