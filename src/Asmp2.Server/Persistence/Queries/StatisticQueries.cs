namespace Asmp2.Server.Persistence.Queries;

public static class StatisticQueries
{
    public static string GenerateStatistics(DateTimeOffset startTimestamp, DateTimeOffset endTimestamp)
    {
        return $@"INSERT INTO Statistics(
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
	            timestamp > str_to_date('{startTimestamp.ToString("yyyy-MM-dd HH:00")}', '%Y-%m-%d %H:00:00')
                AND timestamp < str_to_date('{endTimestamp.ToString("yyyy-MM-dd HH:00")}', '%Y-%m-%d %H:00:00')
            GROUP BY 
	              MeterId
	            , str_to_date(date_format(timestamp, '%Y-%m-%d %H:00:00'),'%Y-%m-%d %H:00:00');";
    }
}
