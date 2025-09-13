namespace MyShop.Domain.Events.Common;
public abstract class BaseDomainEvent : IDomainEvent
{
    public Guid Id { get; }

    public Guid AggregateId { get; }

    public int Version { get; }

    public DateTime OccurredOn { get; }

    public string EventType => GetType().Name;

    protected BaseDomainEvent(Guid aggregateId, int version = 0, DateTime? occurredOn = null)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId != Guid.Empty ? aggregateId : throw new CustomValidationException("AggregateId cannot be empty");
        Version = version >= 0 ? version : throw new CustomValidationException("Version cannot be negative");
        OccurredOn = EnsureUtc(occurredOn ?? DateTime.UtcNow);
    }

    protected BaseDomainEvent(Guid id, Guid aggregateId, int version, DateTime occurredOn)
    {
        Id = id != Guid.Empty ? id : throw new CustomValidationException("Id cannot be empty");
        AggregateId = aggregateId != Guid.Empty ? aggregateId : throw new CustomValidationException("AggregateId cannot be empty");
        Version = version >= 0 ? version : throw new CustomValidationException("Version cannot be negative");
        OccurredOn = EnsureUtc(occurredOn);
    }

    private static DateTime EnsureUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public override string ToString()
    {
        return $"{EventType} (Id: {Id}, AggregateId: {AggregateId}, Version: {Version}, OccurredOn: {OccurredOn:yyyy-MM-dd HH:mm:ss} UTC)";
    }
}
