namespace Asmp2.Server.Core.Model;

public record Measurement(
    Meter Meter,
    DateTime Timestamp,
    PowerReading PowerUsage,
    PowerReading PowerSupply,
    GasReading GasUsage
);