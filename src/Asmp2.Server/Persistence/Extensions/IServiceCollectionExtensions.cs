using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Persistence.Contexts;
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
            .AddDbContexts(configuration);
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IMeasurementRepository, MeasurementRepository>();
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("connectionString");
        
        if(connectionString == null)
        {
            throw new InvalidOperationException("Cannot start persistence layer since the connectionstring setting is not set");
        }

        return services
            .AddDbContextPool<MeasurementContext>(options => {
                options.UseMySQL(connectionString);
            });
    }
}
