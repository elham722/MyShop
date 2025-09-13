namespace MyShop.Contracts.Common.Filtering;

/// <summary>
/// Enumeration of supported filter operators
/// </summary>
public enum FilterOperator
{
    Equals,
    NotEquals,
    Contains,
    NotContains,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    In,
    NotIn,
    IsNull,
    IsNotNull,
    IsEmpty,
    IsNotEmpty
}

/// <summary>
/// DTO for filtering parameters
/// </summary>
public class FilterDto
{
    /// <summary>
    /// The field name to filter by
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// The filter operator
    /// </summary>
    public FilterOperator Operator { get; set; } = FilterOperator.Equals;

    /// <summary>
    /// The filter value
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Additional values for operators like In, NotIn
    /// </summary>
    public IReadOnlyList<string> Values { get; set; } = new List<string>();

    /// <summary>
    /// Validates the filter parameters
    /// </summary>
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Field))
            return false;

        return Operator switch
        {
            FilterOperator.IsNull or FilterOperator.IsNotNull or FilterOperator.IsEmpty or FilterOperator.IsNotEmpty => true,
            FilterOperator.In or FilterOperator.NotIn => Values.Any(),
            _ => !string.IsNullOrWhiteSpace(Value)
        };
    }

    /// <summary>
    /// Creates a filter for equality
    /// </summary>
    public static FilterDto Equals(string field, string value)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Equals, Value = value };
    }

    /// <summary>
    /// Creates a filter for contains
    /// </summary>
    public static FilterDto Contains(string field, string value)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Contains, Value = value };
    }

    /// <summary>
    /// Creates a filter for greater than
    /// </summary>
    public static FilterDto GreaterThan(string field, string value)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.GreaterThan, Value = value };
    }

    /// <summary>
    /// Creates a filter for less than
    /// </summary>
    public static FilterDto LessThan(string field, string value)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.LessThan, Value = value };
    }

    /// <summary>
    /// Creates a filter for In operator
    /// </summary>
    public static FilterDto In(string field, params string[] values)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.In, Values = values.ToList().AsReadOnly() };
    }

    /// <summary>
    /// Creates a filter for IsNull operator
    /// </summary>
    public static FilterDto IsNull(string field)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.IsNull };
    }

    /// <summary>
    /// Creates a filter for IsNotNull operator
    /// </summary>
    public static FilterDto IsNotNull(string field)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.IsNotNull };
    }
}

/// <summary>
/// Collection of filter parameters
/// </summary>
public class FilterDtoCollection : List<FilterDto>
{
    /// <summary>
    /// Creates a new FilterDtoCollection from a single filter
    /// </summary>
    public static FilterDtoCollection FromSingle(string field, FilterOperator @operator, string value = "")
    {
        return new FilterDtoCollection { new FilterDto { Field = field, Operator = @operator, Value = value } };
    }

    /// <summary>
    /// Creates a new FilterDtoCollection from multiple filters
    /// </summary>
    public static FilterDtoCollection FromMultiple(params FilterDto[] filters)
    {
        var collection = new FilterDtoCollection();
        collection.AddRange(filters);
        return collection;
    }

    /// <summary>
    /// Validates all filter parameters
    /// </summary>
    public bool IsValid()
    {
        return this.All(filter => filter.IsValid());
    }
}