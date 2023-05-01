using Asmp2.Server.Persistence.Models;

namespace Asmp2.Server.Persistence.Mappers;

public interface IMeasurementMapper
{
    public MeasurementEntity MapModelToEntity(Measurement source);
}
