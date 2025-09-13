namespace MyShop.Contracts.Common.Sorting;

/// <summary>
/// DTO for sorting parameters
/// </summary>
public class SortDto
{
    /// <summary>
    /// The field name to sort by
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// The sort direction (asc or desc)
    /// </summary>
    public string Direction { get; set; } = "asc";

    /// <summary>
    /// Gets the sort direction as boolean (true for ascending, false for descending)
    /// </summary>
    public bool IsAscending => Direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the sort direction as boolean (true for descending, false for ascending)
    /// </summary>
    public bool IsDescending => Direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Validates the sort parameters
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Field) && 
               (Direction.Equals("asc", StringComparison.OrdinalIgnoreCase) || 
                Direction.Equals("desc", StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// Collection of sort parameters
/// </summary>
public class SortDtoCollection : List<SortDto>
{
    /// <summary>
    /// Creates a new SortDtoCollection from a single sort parameter
    /// </summary>
    public static SortDtoCollection FromSingle(string field, string direction = "asc")
    {
        return new SortDtoCollection { new SortDto { Field = field, Direction = direction } };
    }

    /// <summary>
    /// Creates a new SortDtoCollection from multiple sort parameters
    /// </summary>
    public static SortDtoCollection FromMultiple(params (string field, string direction)[] sorts)
    {
        var collection = new SortDtoCollection();
        foreach (var (field, direction) in sorts)
        {
            collection.Add(new SortDto { Field = field, Direction = direction });
        }
        return collection;
    }

    /// <summary>
    /// Validates all sort parameters
    /// </summary>
    public bool IsValid()
    {
        return this.All(sort => sort.IsValid());
    }
}