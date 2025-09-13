namespace MyShop.Contracts.DomainEvent.Statistics;
public class DomainEventStatistics
{
    public long TotalEventsDispatched { get; set; }

    public long SuccessfulEvents { get; set; }

    public long FailedEvents { get; set; }

    public long RetriedEvents { get; set; }

    public double AverageDispatchTimeMs { get; set; }

    public DateTime? LastDispatchTime { get; set; }

    public double SuccessRate => TotalEventsDispatched > 0 ? (double)SuccessfulEvents / TotalEventsDispatched * 100 : 0;
}