using Asmp2.Server.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Contexts;

public class MeasurementContext : DbContext
{
    public DbSet<MeterEntity> Meters { get; set; }
    public DbSet<MeasurementEntity> Measurements { get; set; }
}
