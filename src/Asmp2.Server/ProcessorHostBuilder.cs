using Asmp2.Server.Application.Extensions;
using Asmp2.Server.Core.Processors;
using Asmp2.Server.Infrastructure.Extensions;
using Asmp2.Server.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server;
public static class ProcessorHostBuilder
{
    public static IProcessorHost Build()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var provider = new ServiceCollection()
            .AddApplicationServices()
            .AddInfrastructureServices()
            .AddPersistance(configuration)
            .BuildServiceProvider();

        return provider.GetRequiredService<IProcessorHost>();
    }
}
