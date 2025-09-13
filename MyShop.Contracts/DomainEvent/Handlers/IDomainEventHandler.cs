namespace MyShop.Contracts.DomainEvent.Handlers;
public interface IDomainEventHandler<in TEvent> where TEvent : BaseDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
