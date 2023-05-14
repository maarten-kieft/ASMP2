using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Server.Persistence.Mappers;
using Asmp2.Server.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore;

namespace Asmp2.Server.Persistence.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRepositories()
            .AddDbContexts(configuration)
            .AddMappers();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IMeasurementRepository, MeasurementRepository>()
            .AddTransient<IStatisticRepository, StatisticRepository>()
            .AddTransient<IMeterRepository, MeterRepository>();
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("asmp:connectionstring")
            ?? throw new InvalidOperationException("Cannot start persistence layer since the connectionstring setting is not set");
        
        return services
            .AddDbContext<AsmpContext>(options => {
                options.UseMySQL(connectionString);
            },
            ServiceLifetime.Transient
        );
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services
            .AddTransient<IMeasurementMapper, MeasurementMapper>();
    }
}
