﻿namespace MyShop.ExternalServices.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime LocalNow => DateTime.Now;

    public DateTime UtcToday => DateTime.UtcNow.Date;

    public DateTime LocalToday => DateTime.Now.Date;

    public DateTime ToLocalTime(DateTime utcDateTime)
    {
        return utcDateTime.ToLocalTime();
    }

    public DateTime ToUtcTime(DateTime localDateTime)
    {
        return localDateTime.ToUniversalTime();
    }
}