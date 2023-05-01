namespace Asmp2.Shared.Model;

public record Statistic(
    Meter Meter,
    DateTimeOffset TimestampStart,
    DateTimeOffset TimestampEnd,
    PowerStatistic PowerUsage,
    PowerStatistic PowerSupply,
    GasStatistic GasUsage
);