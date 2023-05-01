using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Models;

[Index(nameof(Timestamp))]
public class MeasurementEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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
