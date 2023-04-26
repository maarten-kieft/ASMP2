using Asmp2.Server.Application.Processors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

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

        var processorHost = ProcessorHostBuilder.Build();
       
        Task.WhenAll(
            webHost.RunAsync(),
            processorHost.RunAsync()
        ).GetAwaiter().GetResult();
    }
}
