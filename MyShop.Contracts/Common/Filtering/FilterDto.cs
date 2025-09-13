namespace MyShop.Contracts.Common.Filtering;

public class FilterDto
{
    public string Field { get; set; } = string.Empty;

    public FilterOperator Operator { get; set; } = FilterOperator.Equals;

    public string Value { get; set; } = string.Empty;

    public string? ValueType { get; set; }

    public object? RawValue { get; set; }

    public IReadOnlyList<string> Values { get; set; } = new List<string>();

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

    public static FilterDto Equals(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Equals, Value = value, ValueType = valueType };
    }

    public static FilterDto Contains(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.Contains, Value = value, ValueType = valueType };
    }

    public static FilterDto GreaterThan(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.GreaterThan, Value = value, ValueType = valueType };
    }

    public static FilterDto LessThan(string field, string value, string? valueType = null)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.LessThan, Value = value, ValueType = valueType };
    }

    public static FilterDto In(string field, params string[] values)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.In, Values = values.ToList().AsReadOnly() };
    }

    public static FilterDto IsNull(string field)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.IsNull };
    }

    public static FilterDto IsNotNull(string field)
    {
        return new FilterDto { Field = field, Operator = FilterOperator.IsNotNull };
    }
}

