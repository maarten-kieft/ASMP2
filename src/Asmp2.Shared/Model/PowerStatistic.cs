namespace Asmp2.Shared.Model;

public record PowerStatistic(
    decimal TotalLowStart,
    decimal TotalLowEnd,
    decimal TotalRegularStart,
    decimal TotalRegularEnd
);