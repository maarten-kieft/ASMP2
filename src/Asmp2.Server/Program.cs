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
        var webHost = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build();

        var configuration = webHost
            .Services
            .GetService(typeof(IConfiguration)) as IConfiguration 
            ?? throw new InvalidOperationException("Unable to resolve configuration");

        var processorHost = ProcessorHostBuilder
            .Build(configuration!);

        ConnectHubsToMessageBroker(webHost, processorHost);

        Task.WhenAll(
            webHost.RunAsync(),
            processorHost.RunAsync()
        ).GetAwaiter().GetResult();
    }

    private static void ConnectHubsToMessageBroker(IHost webHost, IProcessorHost processorHost)
    {
        var hubContext = webHost.Services.GetService(typeof(IHubContext<MeasurementHub>)) as IHubContext<MeasurementHub>
            ?? throw new InvalidOperationException("Unable to resolve hubcontext");

        var messageBroker = processorHost.Services.GetService(typeof(IMessageBroker)) as IMessageBroker
            ?? throw new InvalidOperationException("Unable to resolve messagebroker");

        messageBroker.Subscribe<MeasurementMessage>((message) =>
        {
            hubContext.Clients.All.SendAsync("Measurement", message.Measurement);
        });
    }
}
