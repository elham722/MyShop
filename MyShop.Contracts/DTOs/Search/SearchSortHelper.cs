using MyShop.Contracts.Common.Sorting;

namespace MyShop.Contracts.DTOs.Search;

/// <summary>
/// Helper class for converting between different sort representations
/// </summary>
public static class SearchSortHelper
{
    /// <summary>
    /// Converts Expression-based sort to SortDto
    /// </summary>
    public static SortDto ToSortDto<T>(Expression<Func<T, object>> orderBy, bool ascending = true, int priority = 0)
    {
        var fieldName = GetFieldName(orderBy);
        return new SortDto 
        { 
            Field = fieldName, 
            Direction = ascending ? "asc" : "desc" 
        };
    }

    /// <summary>
    /// Converts multiple Expression-based sorts to SortDto array
    /// </summary>
    public static SortDto[] ToSortDtos<T>(params (Expression<Func<T, object>> orderBy, bool ascending)[] sorts)
    {
        return sorts.Select(s => ToSortDto(s.orderBy, s.ascending)).ToArray();
    }

    /// <summary>
    /// Converts SortDto to Expression (for LINQ usage)
    /// </summary>
    public static Expression<Func<T, object>> ToExpression<T>(SortDto sortDto)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortDto.Field);
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(conversion, parameter);
    }

    /// <summary>
    /// Gets field name from Expression
    /// </summary>
    private static string GetFieldName<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        
        if (expression.Body is UnaryExpression unaryExpression && 
            unaryExpression.Operand is MemberExpression unaryMemberExpression)
        {
            return unaryMemberExpression.Member.Name;
        }

        throw new ArgumentException("Expression must be a property access", nameof(expression));
    }
}