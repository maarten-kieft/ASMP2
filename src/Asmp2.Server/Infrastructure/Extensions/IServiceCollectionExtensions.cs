using Asmp2.Server.Application.IO;
using Asmp2.Server.Application.Parsers;
using Asmp2.Server.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.
            AddIOServices(configuration);
    }

    public static IServiceCollection AddIOServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<IMeasurementParser, MeasurementParser>();

        if (configuration.GetValue<bool>("DevelopmentSettings:UseFakeReader"))
        {
            services.AddTransient<ISerialReader, FakeSerialReader>();
        }
        else
        {
            services.AddTransient<ISerialReader, SerialReader>();
        }

        return services;
    }
}
