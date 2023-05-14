using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Server.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Repositories;

public class MeterRepository : IMeterRepository
{
    private readonly AsmpContext _context;

    public MeterRepository(IMeasurementMapper measurementMapper, AsmpContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Meter>> GetMetersAsync()
    {
        var meterEntities = await _context.Meters.ToListAsync();

        return meterEntities.Select(e => new Meter(e.Name!)).ToList();
    }
}
