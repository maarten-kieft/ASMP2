using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
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

        await _context.Database.ExecuteSqlRawAsync(
            $@"INSERT INTO Statistics(
	              MeterId
                , TimestampStart
                , TimestampEnd
                , PowerUsageTotalLowStart
                , PowerUsageTotalLowEnd
	            , PowerUsageTotalRegularStart
                , PowerUsageTotalRegularEnd
                , PowerSupplyTotalLowStart
                , PowerSupplyTotalLowEnd
	            , PowerSupplyTotalRegularStart
                , PowerSupplyTotalRegularEnd
                , GasUsageTotalStart
                , GasUsageTotalEnd
            )
            SELECT 
	              MeterId
	            , str_to_date(date_format(timestamp, '%Y-%m-%d %H:00:00'),'%Y-%m-%d %H:00:00') timestamp_start
                , DATE_ADD(str_to_date(date_format(timestamp, '%Y-%m-%d %H:00:00'),'%Y-%m-%d %H:00:00'), INTERVAL 1 HOUR) timestamp_end
                , min(PowerUsageTotalLow) PowerUsageTotalLowStart
                , max(PowerUsageTotalLow) PowerUsageTotalLowEnd
                , min(PowerUsageTotalRegular) PowerUsageTotalRegularStart
                , max(PowerUsageTotalRegular) PowerUsageTotalRegularEnd
	            , min(PowerSupplyTotalLow) PowerSupplyTotalLowStart
                , max(PowerSupplyTotalLow) PowerSupplyTotalLowEnd
                , min(PowerSupplyTotalRegular) PowerSupplyTotalRegularStart
                , max(PowerSupplyTotalRegular) PowerSupplyTotalRegularEnd
	            , min(GasTotal) GasTotalStart
                , max(GasTotal) GasTotalEnd
            FROM 
                Measurements
            WHERE
	            timestamp > {startTimestamp}
                AND timestamp < {endTimestamp}
            GROUP BY 
	              MeterId
	            , str_to_date(date_format(timestamp, '%Y-%m-%d %H:00:00'),'%Y-%m-%d %H:00:00');"
        );
    }
}
