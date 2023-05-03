using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Entities;

public class StatisticEntity
{
    public int Id { get; set; }

    public MeterEntity? Meter { get; set; }
    
    public DateTimeOffset TimestampStart{ get; set; }
    
    public DateTimeOffset TimestampEnd { get; set; }

    [Precision(18, 3)]
    public decimal PowerUsageTotalLowStart { get; set; }

    [Precision(18, 3)]
    public decimal PowerUsageTotalLowEnd { get; set; }

    [Precision(18, 3)]
    public decimal PowerUsageTotalRegularStart { get; set; }
    
    [Precision(18, 3)]
    public decimal PowerUsageTotalRegularEnd { get; set; }
    
    [Precision(18, 3)]
    public decimal PowerSupplyTotalLowStart { get; set; }
    
    [Precision(18, 3)]
    public decimal PowerSupplyTotalLowEnd { get; set; }
    
    [Precision(18, 3)]
    public decimal PowerSupplyTotalRegularStart { get; set; }
    
    [Precision(18, 3)]
    public decimal PowerSupplyTotalRegularEnd { get; set; }
    
    [Precision(18, 3)]
    public decimal GasUsageTotalStart { get; set; }
    
    [Precision(18, 3)]
    public decimal GasUsageTotalEnd { get; set; }

}
