using Asmp2.Server.Persistence.Models;

namespace Asmp2.Server.Persistence.Mappers;

public class MeasurementMapper : IMeasurementMapper
{
    public MeasurementEntity MapModelToEntity(Measurement source)
    {
        return new MeasurementEntity
        {
            Id = 1,
            Meter = new MeterEntity { Id =1, Name = source.Meter.Id },
            Timestamp = source.Timestamp,
            PowerUsageCurrent = source.PowerUsage.Current,
            PowerUsageTotalLow = source.PowerUsage.TotalLow,
            PowerUsageTotalRegular = source.PowerUsage.TotalRegular,
            PowerSupplyCurrent = source.PowerSupply.Current,
            PowerSupplyTotalLow = source.PowerSupply.TotalLow,
            PowerSupplyTotalRegular = source.PowerSupply.TotalRegular,
            GasOpen = source.GasUsage?.Open ?? false,
            GasTotal = source.GasUsage?.Total ?? 0,
        };
    }
}