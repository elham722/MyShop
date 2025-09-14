namespace MyShop.Contracts.Common.Sorting;

public static class SortingHelper
{
    public static List<SortDto> CreateSingle(string field, string direction = "asc")
    {
        return new List<SortDto> { new SortDto { Field = field, Direction = direction } };
    }

    public static List<SortDto> CreateMultiple(params SortDto[] sorts)
    {
        return sorts.ToList();
    }

    public static List<SortDto> CreateMultiple(IEnumerable<SortDto> sorts)
    {
        return sorts.ToList();
    }

    public static List<SortDto> CreateMultiple(params (string field, string direction)[] sorts)
    {
        return sorts.Select(s => new SortDto { Field = s.field, Direction = s.direction }).ToList();
    }

    public static bool IsValid(this IEnumerable<SortDto> sorts)
    {
        return sorts.All(sort => sort.IsValid());
    }

    public static IEnumerable<SortDto> GetInvalidSorts(this IEnumerable<SortDto> sorts)
    {
        return sorts.Where(sort => !sort.IsValid());
    }

    public static IEnumerable<SortDto> GetValidSorts(this IEnumerable<SortDto> sorts)
    {
        return sorts.Where(sort => sort.IsValid());
    }

    public static List<SortDto> Empty()
    {
        return new List<SortDto>();
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
}