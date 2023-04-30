namespace Asmp2.Shared.Model;

public record Measurement(
    Meter Meter,
    DateTimeOffset Timestamp,
    PowerReading PowerUsage,
    PowerReading PowerSupply,
    GasReading? GasUsage = default
);