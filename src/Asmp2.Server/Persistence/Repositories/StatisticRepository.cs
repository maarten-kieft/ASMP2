using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;

namespace Asmp2.Server.Persistence.Repositories;

public class StatisticRepository : IStatisticRepository
{
    private readonly AsmpContext _context;

    public StatisticRepository(AsmpContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task CreateStatisticsAsync()
    {

        var startTimestamp = _context.Statistics
            .OrderByDescending(s => s.TimestampEnd)
            .FirstOrDefault()
            ?.TimestampEnd ?? new DateTimeOffset(new DateTime(2000, 1, 1));

        var endTimestamp = new DateTimeOffset(
            new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                0,
                0
            )
        );

        var statistics = _context.Measurements
            .Where(m => m.Timestamp >= startTimestamp && m.Timestamp < endTimestamp)
            .GroupBy(m => m.Timestamp.ToString("yyyyMMddhh"))
            ;

        var a = 1;

        return Task.CompletedTask;
    }
}
