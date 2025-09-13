namespace MyShop.Contracts.Common.Sorting;

public class SortDtoCollection : List<SortDto>
{
    public static SortDtoCollection FromSingle(string field, string direction = "asc")
    {
        return new SortDtoCollection { new SortDto { Field = field, Direction = direction } };
    }

    public static SortDtoCollection FromMultiple(params (string field, string direction)[] sorts)
    {
        var collection = new SortDtoCollection();
        foreach (var (field, direction) in sorts)
        {
            collection.Add(new SortDto { Field = field, Direction = direction });
        }
        return collection;
    }

    public bool IsValid()
    {
        return this.All(sort => sort.IsValid());
    }
}