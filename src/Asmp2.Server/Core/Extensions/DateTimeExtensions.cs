namespace Asmp2.Server.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset CalculatePeriodEndDateTime(this DateTimeOffset dateTime, Period period)
    {
        return period switch
        {
            Period.Day => dateTime.AddDays(1),
            Period.Month => dateTime.AddMonths(1),
            Period.Year => dateTime.AddYears(1),
            _ => throw new NotImplementedException($"The period type '{period} is not suppored by AddPeriod'"),
        };
    }

    public static DateTimeOffset Truncate(this DateTimeOffset dateTime, Period period)
    {
        return period switch
        {
            Period.Day => dateTime,
            Period.Month => new DateTimeOffset(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)),
            Period.Year => new DateTimeOffset(new DateTime(dateTime.Year, dateTime.Month, 1)),
            _ => throw new NotImplementedException($"The period type '{period} is not suppored by Truncate'"),
        };
    }
}
