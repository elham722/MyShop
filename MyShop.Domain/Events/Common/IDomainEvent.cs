namespace MyShop.Domain.Events.Common;
public interface IDomainEvent
{
    Guid Id { get; }

    Guid AggregateId { get; }

    int Version { get; }

    DateTime OccurredOn { get; }

    string EventType { get; }
}
