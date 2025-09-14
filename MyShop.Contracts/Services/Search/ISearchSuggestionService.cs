using MyShop.Contracts.DTOs.Search;
using MyShop.Domain.Shared.Base;

namespace MyShop.Contracts.Services.Search;

/// <summary>
/// Service for search suggestions and autocomplete
/// </summary>
public interface ISearchSuggestionService<T, TId> where T : BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    #region Search Suggestions

    /// <summary>
    /// Gets search suggestions based on partial input
    /// </summary>
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets search suggestions for specific fields
    /// </summary>
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm, Expression<Func<T, object>>[] searchFields, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets contextual search suggestions
    /// </summary>
    Task<IEnumerable<SearchSuggestion>> GetContextualSuggestionsAsync(string searchTerm, string context, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets trending search suggestions
    /// </summary>
    Task<IEnumerable<string>> GetTrendingSuggestionsAsync(int maxSuggestions = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Autocomplete

    /// <summary>
    /// Gets autocomplete suggestions
    /// </summary>
    Task<IEnumerable<AutocompleteSuggestion>> GetAutocompleteAsync(string query, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets autocomplete suggestions for specific field
    /// </summary>
    Task<IEnumerable<AutocompleteSuggestion>> GetAutocompleteAsync(string query, Expression<Func<T, object>> field, int maxSuggestions = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Suggestion Management

    /// <summary>
    /// Adds a custom search suggestion
    /// </summary>
    Task AddCustomSuggestionAsync(string suggestion, string category = "custom", CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a custom search suggestion
    /// </summary>
    Task RemoveCustomSuggestionAsync(string suggestion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets custom suggestions by category
    /// </summary>
    Task<IEnumerable<string>> GetCustomSuggestionsAsync(string category, CancellationToken cancellationToken = default);

    #endregion

    #region Suggestion Configuration

    /// <summary>
    /// Updates suggestion configuration
    /// </summary>
    Task UpdateSuggestionConfigurationAsync(SearchSuggestionConfiguration configuration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets suggestion configuration
    /// </summary>
    Task<SearchSuggestionConfiguration> GetSuggestionConfigurationAsync(CancellationToken cancellationToken = default);

    #endregion
}