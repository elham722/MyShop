namespace MyShop.Domain.Services;
public interface IDomainEventService
{
    Task PublishAsync(BaseDomainEvent domainEvent);
    Task PublishAsync(IEnumerable<BaseDomainEvent> domainEvents);
}
