using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Core.Extensions;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Server.Persistence.Queries;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Repositories;

public class StatisticRepository : IStatisticRepository
{
    private readonly AsmpContext _context;

    public StatisticRepository(AsmpContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateStatisticsAsync()
    {
        var startTimestamp = _context.Statistics
            .OrderByDescending(s => s.TimestampEnd)
            .FirstOrDefault()
            ?.TimestampEnd ?? new DateTimeOffset(new DateTime(2000, 1, 1));

        var endTimestamp = DateTimeOffset.UtcNow;
        var query = StatisticQueries.GenerateStatistics(startTimestamp, endTimestamp);

        await _context.Database.ExecuteSqlRawAsync(query);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Statistic> GetStatistics(string meterId, Period period, DateTimeOffset startDateTime)
    {
        var endDateTime = startDateTime.CalculatePeriodEndDateTime(period);

        return _context.Statistics
            .Where(s => s.Meter.Name == meterId)
            .Where(s => s.TimestampStart >= startDateTime && s.TimestampEnd < endDateTime)
            .GroupBy(s =>
                new
                {
                    Meter = s.Meter!.Name ?? "Unkown",
                    Month = s.TimestampStart.Month,
                    Day = period == Period.Month ? s.TimestampStart.Day : 0,
                    Hour = period == Period.Day ? s.TimestampStart.Hour : 0
                }
            )
             .Select(sg => 
                 new Statistic(
                     new Meter(sg.Key.Meter),
                     sg.Min(s => s.TimestampStart),
                     sg.Max(s => s.TimestampEnd),
                     new PowerStatistic(
                         sg.Min(s => s.PowerUsageTotalLowStart),
                         sg.Max(s => s.PowerUsageTotalLowEnd),
                         sg.Min(s => s.PowerUsageTotalRegularStart),
                         sg.Max(s => s.PowerUsageTotalRegularEnd)
                     ),
                     new PowerStatistic(
                         sg.Min(s => s.PowerSupplyTotalLowStart),
                         sg.Max(s => s.PowerSupplyTotalLowEnd),
                         sg.Min(s => s.PowerSupplyTotalRegularStart),
                         sg.Max(s => s.PowerSupplyTotalRegularEnd)
                     ),
                     new GasStatistic(
                         sg.Min(s => s.GasUsageTotalStart),
                         sg.Max(s => s.GasUsageTotalEnd)
                     )
                 )
         )
         .ToList(); 
    }
}
