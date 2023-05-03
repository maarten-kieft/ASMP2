namespace Asmp2.Server.Application.Repositories;

public interface IMeasurementRepository
{
    Task SaveMeasurementAsync(Measurement measurement);
    Task CleanMeasurementsAsync();
}
