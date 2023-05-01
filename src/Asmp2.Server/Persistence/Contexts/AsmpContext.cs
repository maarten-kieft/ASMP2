using Asmp2.Server.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Contexts;

public class AsmpContext : DbContext
{
    public DbSet<MeterEntity> Meters { get; set; }
    public DbSet<MeasurementEntity> Measurements { get; set; }

    public AsmpContext(DbContextOptions<AsmpContext> options) : base(options)
    {

    }
}
