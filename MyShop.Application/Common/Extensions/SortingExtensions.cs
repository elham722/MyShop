namespace MyShop.Application.Common.Extensions;

public static class SortingExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, SortDto sortDto)
    {
        if (sortDto == null || !sortDto.IsValid())
            return query;

        var direction = sortDto.IsAscending ? "asc" : "desc";
        var orderByExpression = $"{sortDto.Field} {direction}";
        
        return query.OrderBy(orderByExpression);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, SortDtoCollection sortDtos)
    {
        if (sortDtos == null || !sortDtos.IsValid())
            return query;

        var validSorts = sortDtos.Where(s => s.IsValid()).ToList();
        if (!validSorts.Any())
            return query;

        IQueryable<T> sortedQuery = query;
        bool isFirst = true;

        foreach (var sortDto in validSorts)
        {
            var direction = sortDto.IsAscending ? "asc" : "desc";
            var orderByExpression = $"{sortDto.Field} {direction}";

            if (isFirst)
            {
                sortedQuery = sortedQuery.OrderBy(orderByExpression);
                isFirst = false;
            }
            else
            {
                sortedQuery = ((IOrderedQueryable<T>)sortedQuery).ThenBy(orderByExpression);
            }
        }

        return sortedQuery;
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string field, string direction = "asc")
    {
        if (string.IsNullOrWhiteSpace(field))
            return query;

        var sortDto = new SortDto { Field = field, Direction = direction };
        return query.ApplySorting(sortDto);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string field, bool ascending = true)
    {
        var direction = ascending ? "asc" : "desc";
        return query.ApplySorting(field, direction);
    }
}