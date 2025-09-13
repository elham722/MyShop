namespace MyShop.Contracts.DomainEvent.Dispatching;

public interface IDomainEventDispatcher
{
    #region Basic Event Dispatching

    Task DispatchAsync(BaseDomainEvent domainEvent, CancellationToken cancellationToken = default);

    Task DispatchAsync(IEnumerable<BaseDomainEvent> domainEvents, CancellationToken cancellationToken = default);

    #endregion

    #region Error Handling

    Task DispatchAsync(IEnumerable<BaseDomainEvent> domainEvents, Func<BaseDomainEvent, Exception, Task> onError, CancellationToken cancellationToken = default);

    Task DispatchWithRetryAsync(IEnumerable<BaseDomainEvent> domainEvents, int maxRetries = 3, TimeSpan delayBetweenRetries = default, CancellationToken cancellationToken = default);

    #endregion

    #region Event Management

    IEnumerable<Type> GetEventHandlers<TEvent>() where TEvent : BaseDomainEvent;

    bool HasHandlers<TEvent>() where TEvent : BaseDomainEvent;

    int GetHandlerCount();

    #endregion

    #region Event Statistics

    Task<DomainEventStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);

    Task ResetStatisticsAsync(CancellationToken cancellationToken = default);

    #endregion
}