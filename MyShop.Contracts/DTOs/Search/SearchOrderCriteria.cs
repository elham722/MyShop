namespace MyShop.Contracts.DTOs.Search;

public class SearchOrderCriteria
{
    public Expression<Func<object, object>> OrderBy { get; set; } = null!;
    
    public bool Ascending { get; set; } = true;
    
    public int Priority { get; set; } = 0;
}