namespace Asmp2.Shared.Model;

public record Measurement(
    Meter Meter,
    DateTime Timestamp,
    PowerReading PowerUsage,
    PowerReading PowerSupply,
    GasReading GasUsage
);