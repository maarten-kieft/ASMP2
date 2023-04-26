using Asmp2.Server.Core.Processors;
using System.Linq;

namespace Asmp2.Server.Application.Processors;
public class ProcessorHost : IProcessorHost
{
    private readonly IEnumerable<IProcessor> _processors;

    public ProcessorHost(IEnumerable<IProcessor> processors)
    {
        _processors = processors ?? throw new ArgumentNullException(nameof(processors));
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(_processors.Select(p => p.RunAsync(cancellationToken)));
    }
}
