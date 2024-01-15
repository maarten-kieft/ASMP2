using Asmp2.Server.Application.Extensions;
using Asmp2.Server.Application.Processors;
using Asmp2.Server.Infrastructure.Extensions;
using Asmp2.Server.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server;
public static class ProcessorHostBuilder
{
    public static IProcessorHost Build(IConfiguration configuration)
    {
        var provider = new ServiceCollection()
            .AddLogging(builder => {
                builder.AddConfiguration(configuration.GetSection("logging"));
                builder.AddConsole(); 
            })
            .AddApplicationServices()
            .AddInfrastructureServices(configuration)
            .AddPersistance(configuration)
            .BuildServiceProvider();

        return provider.GetRequiredService<IProcessorHost>();
    }
}
