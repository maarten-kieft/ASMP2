namespace Asmp2.Server.Core.Model;

public record Statistic(
    Meter Meter,
    DateTime TimestampStart,
    DateTime TimestampEnd,
    PowerReading PowerUsage,
    PowerReading PowerSupply,
    GasReading GasUsage
);