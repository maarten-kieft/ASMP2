using Asmp2.Server.Persistence.Entities;

namespace Asmp2.Server.Persistence.Mappers;

public interface IMeasurementMapper
{
    public MeasurementEntity MapModelToEntity(Measurement source);
}
