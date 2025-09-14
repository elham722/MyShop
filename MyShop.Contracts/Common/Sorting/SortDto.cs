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

    public static SortDto Ascending(string field)
    {
        return new SortDto { Field = field, Direction = "asc" };
    }

    public static SortDto Descending(string field)
    {
        return new SortDto { Field = field, Direction = "desc" };
    }

    public static SortDto Create(string field, string direction)
    {
        return new SortDto { Field = field, Direction = direction };
    }

    public static SortDto Create(string field, bool ascending)
    {
        return new SortDto { Field = field, Direction = ascending ? "asc" : "desc" };
    }

    public override string ToString()
    {
        return $"{Field} {Direction}";
    }
}