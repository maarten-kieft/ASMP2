using Asmp2.Client;
using Asmp2.Server.Core.Processors;
using Asmp2.Server.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

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
        var context = Services.GetService<AsmpContext>();
        
        try
        {
            if(context == null)
            {
                throw new InvalidOperationException("Not able to resolve asmp context");
            }

            context.Database.EnsureCreated();

            await Task.WhenAll(processors.Select(p => p.RunAsync(cancellationToken)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
