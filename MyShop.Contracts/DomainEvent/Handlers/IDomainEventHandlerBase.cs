namespace MyShop.Contracts.DomainEvent.Handlers;
public interface IDomainEventHandler
{
    Task HandleAsync(BaseDomainEvent domainEvent, CancellationToken cancellationToken = default);

    bool CanHandle(Type eventType);
}
