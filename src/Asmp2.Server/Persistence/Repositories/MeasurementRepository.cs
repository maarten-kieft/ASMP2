using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Server.Persistence.Mappers;
using Asmp2.Server.Persistence.Models;

namespace Asmp2.Server.Persistence.Repositories;

public class MeasurementRepository : IMeasurementRepository
{
    private readonly IMeasurementMapper _measurementMapper;
    private readonly AsmpContext _context;

    public MeasurementRepository(IMeasurementMapper measurementMapper, AsmpContext context)
    {
        _measurementMapper = measurementMapper ?? throw new ArgumentNullException(nameof(measurementMapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SaveMeasurementAsync(Measurement measurement)
    {
        var meterEntity = _context.Meters.FirstOrDefault(m => m.Name == measurement.Meter.Id) ?? new MeterEntity { Name = measurement.Meter.Id };
        var measurementEntity = _measurementMapper.MapModelToEntity(measurement);

        if (!meterEntity.Id.HasValue)
        {
            _context.Meters.Add(meterEntity);
            await _context.SaveChangesAsync();
        }

        meterEntity.Measurements.Add(measurementEntity);
        await _context.SaveChangesAsync();
    }
}
