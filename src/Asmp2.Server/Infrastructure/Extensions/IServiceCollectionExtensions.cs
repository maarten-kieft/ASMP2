using Asmp2.Server.Application.IO;
using Asmp2.Server.Application.Parsers;
using Asmp2.Server.Core.Parsers;
using Asmp2.Server.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.
            AddIOServices();
    }

    public static IServiceCollection AddIOServices(this IServiceCollection services)
    {
        return services
            .AddTransient<ISerialReader, SerialReader>()
            .AddTransient<IMeasurementParser, MeasurementParser>();
    }
}
