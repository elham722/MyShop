namespace MyShop.Contracts.DomainEvent.Exceptions;
public class DomainEventHandlerRegistrationException : Exception
{
    public Type? EventType { get; }

    public Type? HandlerType { get; }

    public DomainEventHandlerRegistrationException(string message) : base(message) { }

    public DomainEventHandlerRegistrationException(string message, Exception innerException) : base(message, innerException) { }

    public DomainEventHandlerRegistrationException(string message, Type eventType, Type handlerType, Exception innerException)
        : base(message, innerException)
    {
        EventType = eventType;
        HandlerType = handlerType;
    }
}
