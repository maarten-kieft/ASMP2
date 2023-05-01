using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Entities;

[Index(nameof(Timestamp))]
public class MeasurementEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public MeterEntity? Meter { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    [Precision(6, 3)]
    public decimal PowerUsageCurrent { get; set; }

    [Precision(18, 3)]
    public decimal PowerUsageTotalLow{ get; set; }

    [Precision(18, 3)]
    public decimal PowerUsageTotalRegular { get; set; }

    [Precision(18, 3)]
    public decimal PowerSupplyCurrent { get; set; }

    [Precision(18, 3)]
    public decimal PowerSupplyTotalLow { get; set; }

    [Precision(18, 3)]
    public decimal PowerSupplyTotalRegular { get; set; }

    public bool GasOpen { get; set; }

    public decimal GasTotal { get; set; }
}
