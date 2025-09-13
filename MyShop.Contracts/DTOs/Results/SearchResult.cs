namespace MyShop.Contracts.DTOs.Results;

public class SearchResult<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    
    public int TotalCount { get; set; }
    
    public SearchStatistics? Statistics { get; set; }
    
    public IEnumerable<string> Suggestions { get; set; } = new List<string>();
    
    public Dictionary<string, string> Highlights { get; set; } = new Dictionary<string, string>();
}