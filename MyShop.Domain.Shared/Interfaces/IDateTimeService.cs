namespace MyShop.Domain.Shared.Interfaces;

public interface IDateTimeService
{
    DateTime UtcNow { get; }

    DateTime LocalNow { get; }

    DateTime UtcToday { get; }

    DateTime LocalToday { get; }

    DateTime ToLocalTime(DateTime utcDateTime);

    DateTime ToUtcTime(DateTime localDateTime);
}