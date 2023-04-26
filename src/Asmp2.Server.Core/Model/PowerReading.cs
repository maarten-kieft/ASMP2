namespace Asmp2.Server.Core.Model;

public record PowerReading(
    decimal Current,
    decimal TotalLow,
    decimal TotalRegular
);