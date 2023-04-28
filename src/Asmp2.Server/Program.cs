using Asmp2.Server.Core.Messaging;
using Asmp2.Server.Core.Processors;
using Asmp2.Server.Web.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Asmp2.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var webHost = BuildWebHost(args);
        var processorHost = ProcessorHostBuilder.Build();

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
        var hubContext = (IHubContext<MeasurementHub>)webHost.Services.GetService(typeof(IHubContext<MeasurementHub>));
        var messageBroker = (IMessageBroker)processorHost.Services.GetService(typeof(IMessageBroker));

        var messageTypes = typeof(Message).Assembly
           .GetTypes()
           .Where(t =>
               t.IsAssignableTo(typeof(Message)) &&
               !t.IsAbstract
           );

        foreach (var messageType in messageTypes)
        {
            messageBroker.Subscribe(messageType, (message) => {
                hubContext.Clients.All.SendAsync("Measurement", ((MeasurementMessage)message).Measurement);
            });
        }
    }
}
