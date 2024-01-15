using Asmp2.Server.Persistence.Contexts;
using Asmp2.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Asmp2.Server.Application.Processors;
public class ProcessorHost : IProcessorHost
{
    private readonly ILogger<ProcessorHost> _logger;

    public ProcessorHost(IServiceProvider services, ILogger<ProcessorHost> logger)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IServiceProvider Services { get; }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Booting processor host");

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
            _logger.LogInformation($"Booting processor '{processor.GetType().Name}'");
            await processor.RunAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Processor {processor.GetType().Name} crashed.");
            _logger.LogError(ex,$"Message: {ex.Message}");
            _logger.LogError(ex,$"Stacktrace: {ex.StackTrace}");
            _logger.LogInformation("Sleeping for 1 minute");
            await Task.Delay(MilliSecondConstants.OneMinute);
            _logger.LogInformation("Restarting processor");

            await RunProcessor(processor, cancellationToken);

        }
    }
}
