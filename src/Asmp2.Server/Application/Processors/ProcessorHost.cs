using Asmp2.Server.Core.Processors;

namespace Asmp2.Server.Application.Processors;
public class ProcessorHost : IProcessorHost
{
    private readonly IEnumerable<IProcessor> _processors;

    public ProcessorHost(IEnumerable<IProcessor> processors)
    {
        _processors = processors ?? throw new ArgumentNullException(nameof(processors));
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.WhenAll(_processors.Select(p => p.RunAsync(cancellationToken)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
