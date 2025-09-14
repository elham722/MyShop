using MyShop.Domain.Shared.Events.Common;

namespace MyShop.Contracts.DomainEvent.Exceptions;
public class DomainEventDispatchException : Exception
{
    public BaseDomainEvent? FailedEvent { get; }

    public Type? FailedHandler { get; }

    public int RetryCount { get; }

    public DomainEventDispatchException(string message) : base(message) { }

    public DomainEventDispatchException(string message, Exception innerException) : base(message, innerException) { }

    public DomainEventDispatchException(string message, BaseDomainEvent failedEvent, Exception innerException)
        : base(message, innerException)
    {
        FailedEvent = failedEvent;
    }

    public DomainEventDispatchException(string message, BaseDomainEvent failedEvent, Type failedHandler, Exception innerException)
        : base(message, innerException)
    {
        FailedEvent = failedEvent;
        FailedHandler = failedHandler;
    }

    public DomainEventDispatchException(string message, BaseDomainEvent failedEvent, Type failedHandler, int retryCount, Exception innerException)
        : base(message, innerException)
    {
        FailedEvent = failedEvent;
        FailedHandler = failedHandler;
        RetryCount = retryCount;
    }
}
