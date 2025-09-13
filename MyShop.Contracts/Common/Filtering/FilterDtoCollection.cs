namespace MyShop.Contracts.Common.Filtering;

public class FilterDtoCollection : List<FilterDto>
{

    public static FilterDtoCollection FromSingle(string field, FilterOperator @operator, string value = "")
    {
        return new FilterDtoCollection { new FilterDto { Field = field, Operator = @operator, Value = value } };
    }

    public static FilterDtoCollection FromMultiple(params FilterDto[] filters)
    {
        var collection = new FilterDtoCollection();
        collection.AddRange(filters);
        return collection;
    }

    public bool IsValid()
    {
        return this.All(filter => filter.IsValid());
    }
}