using Asmp2.Server.Application.Repositories;
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
}
