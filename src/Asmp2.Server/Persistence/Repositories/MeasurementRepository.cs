using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Server.Persistence.Mappers;

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
       var measurementEntity = _measurementMapper.MapModelToEntity(measurement);
        await _context.AddAsync(measurementEntity);
        await _context.SaveChangesAsync();
    }
}
