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
    /// The type of the value for proper parsing (string, int, decimal, datetime, bool, guid)
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// Parsed value for dynamic LINQ (set automatically when ValueType is provided)
    /// </summary>
    public object? RawValue { get; set; }

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
            _ => !string.IsNullOrWhiteSpace(Value) || RawValue != null
        };
    }

    /// <summary>
    /// Parses the Value string to the appropriate type based on ValueType
    /// </summary>
    public object? ParseValue()
    {
        if (RawValue != null) return RawValue;
        if (string.IsNullOrWhiteSpace(Value)) return null;

        return ValueType?.ToLowerInvariant() switch
        {
            "int" or "integer" => int.TryParse(Value, out var intVal) ? intVal : null,
            "long" => long.TryParse(Value, out var longVal) ? longVal : null,
            "decimal" => decimal.TryParse(Value, out var decimalVal) ? decimalVal : null,
            "double" => double.TryParse(Value, out var doubleVal) ? doubleVal : null,
            "float" => float.TryParse(Value, out var floatVal) ? floatVal : null,
            "datetime" or "date" => DateTime.TryParse(Value, out var dateVal) ? dateVal : null,
            "bool" or "boolean" => bool.TryParse(Value, out var boolVal) ? boolVal : null,
            "guid" => Guid.TryParse(Value, out var guidVal) ? guidVal : null,
            "string" or null => Value, // Default to string
            _ => Value // Fallback to string
        };
    }

    /// <summary>
    /// Creates a filter for equality
    /// </summary>
    public static FilterDto Equals(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Equals, Value = value, ValueType = valueType };
    }

    /// <summary>
    /// Creates a filter for contains
    /// </summary>
    public static FilterDto Contains(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Contains, Value = value, ValueType = valueType };
    }

    /// <summary>
    /// Creates a filter for greater than
    /// </summary>
    public static FilterDto GreaterThan(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.GreaterThan, Value = value, ValueType = valueType };
    }

    /// <summary>
    /// Creates a filter for less than
    /// </summary>
    public static FilterDto LessThan(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.LessThan, Value = value, ValueType = valueType };
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