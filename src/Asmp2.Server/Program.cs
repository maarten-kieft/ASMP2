using Asmp2.Server.Core.Messaging;
using Asmp2.Server.Core.Processors;
using Asmp2.Server.Web.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Asmp2.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var webHost = BuildWebHost(args);
        var configuration = webHost.Services.GetService(typeof(IConfiguration)) as IConfiguration;
        var processorHost = ProcessorHostBuilder.Build(configuration);

        ConnectHubsToMessageBroker(webHost, processorHost);

        Task.WhenAll(
            webHost.RunAsync(),
            processorHost.RunAsync()
        ).GetAwaiter().GetResult();
    }

    private static IHost BuildWebHost(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
        .Build();
    }

    private static void ConnectHubsToMessageBroker(IHost webHost, IProcessorHost processorHost)
    {
        var hubContext = webHost.Services.GetService(typeof(IHubContext<MeasurementHub>)) as IHubContext<MeasurementHub>;
        var messageBroker = processorHost.Services.GetService(typeof(IMessageBroker)) as IMessageBroker;

        if (hubContext == null || messageBroker == null)
        {
            throw new InvalidOperationException("Not able to fetch hubcontext and/or messageBroker");
        }

        messageBroker.Subscribe<MeasurementMessage>((message) =>
        {
            hubContext.Clients.All.SendAsync("Measurement", message.Measurement);
        });
    }
}
