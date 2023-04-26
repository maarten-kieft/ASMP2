using Asmp2.Server.Application.Extensions;
using Asmp2.Server.Infrastructure.Extensions;
using Asmp2.Server.Core.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server;
public static class ProcessorHostBuilder
{
    public static IProcessorHost Build()
    {
        var provider = new ServiceCollection()
            .AddApplicationServices()
            .AddInfrastructureServices()
            .BuildServiceProvider();

        return provider.GetRequiredService<IProcessorHost>();
    }
}
