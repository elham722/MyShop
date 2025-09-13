namespace MyShop.Contracts.Common.Sorting;

public class SortDto
{
    public string Field { get; set; } = string.Empty;

    public string Direction { get; set; } = "asc";

    public bool IsAscending => Direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

    public bool IsDescending => Direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Field) && 
               (Direction.Equals("asc", StringComparison.OrdinalIgnoreCase) || 
                Direction.Equals("desc", StringComparison.OrdinalIgnoreCase));
    }
}