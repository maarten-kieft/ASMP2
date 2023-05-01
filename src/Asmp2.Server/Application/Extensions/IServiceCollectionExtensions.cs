using Asmp2.Server.Application.Messaging;
using Asmp2.Server.Application.Processors;
using Asmp2.Server.Core.Messaging;
using Asmp2.Server.Core.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddProcessors()
            .AddMessaging();
    }

    public static IServiceCollection AddProcessors(this IServiceCollection services)
    {
        return services
            .AddTransient<IProcessorHost, ProcessorHost>()
            .AddTransient<IProcessor, Reader>()
            .AddTransient<IProcessor, Aggregator>();
    }

    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMessageBroker, MessageBroker>();
    }
}
