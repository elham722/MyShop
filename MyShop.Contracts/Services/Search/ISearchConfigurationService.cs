using MyShop.Contracts.DTOs.Search;
using MyShop.Domain.Shared.Base;
using SearchConfig = MyShop.Contracts.DTOs.Search.SearchConfiguration;

namespace MyShop.Contracts.Services.Search;

/// <summary>
/// Service for search configuration management
/// </summary>
public interface ISearchConfigurationService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Search Configuration

    /// <summary>
    /// Updates search configuration
    /// </summary>
    Task UpdateSearchConfigurationAsync(SearchConfig configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current search configuration
    /// </summary>
    Task<SearchConfig> GetSearchConfigurationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets search configuration to defaults
    /// </summary>
    Task ResetSearchConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Search Index Management

    /// <summary>
    /// Rebuilds search index
    /// </summary>
    Task RebuildSearchIndexAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates search index for specific entity
    /// </summary>
    Task UpdateSearchIndexAsync(TId entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes entity from search index
    /// </summary>
    Task RemoveFromSearchIndexAsync(TId entityId, CancellationToken cancellationToken = default);

    #endregion

    #region Search Field Configuration

    /// <summary>
    /// Configures searchable fields
    /// </summary>
    Task ConfigureSearchableFieldsAsync(Expression<Func<T, object>>[] fields, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets configured searchable fields
    /// </summary>
    Task<Expression<Func<T, object>>[]> GetSearchableFieldsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets field search weight
    /// </summary>
    Task SetFieldWeightAsync(Expression<Func<T, object>> field, double weight, CancellationToken cancellationToken = default);

    #endregion
}