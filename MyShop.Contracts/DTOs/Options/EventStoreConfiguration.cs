namespace MyShop.Contracts.DTOs.Options;

public class EventStoreConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;
    
    public string EventsTableName { get; set; } = "Events";
   
    public string SnapshotsTableName { get; set; } = "Snapshots";
    
    public bool EnableSnapshots { get; set; } = true;

    public int SnapshotInterval { get; set; } = 100;
    
    public bool EnableCompression { get; set; } = false;
    
    public bool EnableEncryption { get; set; } = false;
    
    public string? EncryptionKey { get; set; }
    
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    
    public int MaxRetryCount { get; set; } = 3;
    
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);
}