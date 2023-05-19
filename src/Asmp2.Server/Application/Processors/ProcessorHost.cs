using Asmp2.Client;
using Asmp2.Server.Core.Processors;
using Asmp2.Server.Persistence.Contexts;
using Asmp2.Shared.Constants;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Asmp2.Server.Application.Processors;
public class ProcessorHost : IProcessorHost
{
    public ProcessorHost(IServiceProvider services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceProvider Services { get; }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var processors = Services.GetServices<IProcessor>();
        EnsureEnvironment();

        await Task.WhenAll(processors.Select(p => RunProcessor(p, cancellationToken)));
    }

    private void EnsureEnvironment()
    {
        var context = Services.GetService<AsmpContext>()
            ?? throw new InvalidOperationException("Not able to resolve asmp context");
        
        context.Database.EnsureCreated();
    }

    private async Task RunProcessor(IProcessor processor, CancellationToken cancellationToken)
    {
        try
        {
            await processor.RunAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Processor {processor.GetType().Name} crashed: {ex.Message}, stack: {ex.StackTrace}");
            Console.WriteLine("Sleeping for 1 minute");
            await Task.Delay(MilliSecondConstants.OneMinute);
            Console.WriteLine("Restarting processor");
            
            await RunProcessor(processor, cancellationToken);

        }
    }
}
