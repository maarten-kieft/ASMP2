namespace Asmp2.Server.Persistence.Models;

public class MeasurementEntity
{
    public MeterEntity Meter { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public decimal PowerUsageCurrent { get; set; }
    public decimal PowerUsageTotalLow{ get; set; }
    public decimal PowerUsageTotalRegular { get; set; }
    public decimal PowerSupplyCurrent { get; set; }
    public decimal PowerSupplyTotalLow { get; set; }
    public decimal PowerSupplyTotalRegular { get; set; }
    public bool? GasOpen { get; set; }
    public decimal? GasTotal { get; set; }
}
