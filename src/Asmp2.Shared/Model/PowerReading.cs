namespace Asmp2.Shared.Model;

public record PowerReading(
    decimal Current,
    decimal TotalLow,
    decimal TotalRegular
);