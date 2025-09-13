namespace MyShop.Contracts.DomainEvent.Statistics;
public class DomainEventStatisticsException : Exception
{
    public DomainEventStatisticsException(string message) : base(message) { }

    public DomainEventStatisticsException(string message, Exception innerException) : base(message, innerException) { }
}