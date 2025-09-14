namespace MyShop.Contracts.DTOs.Search;

public class PopularSearchTerm
{
    public string Term { get; set; } = string.Empty;
    
    public long SearchCount { get; set; }
    
    public DateTime LastSearched { get; set; }
    
    public double AverageResults { get; set; }
}