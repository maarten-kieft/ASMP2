namespace Asmp2.Server.Core.Processors;

public interface IProcessorHost
{
    public IServiceProvider Services { get; }

    public Task RunAsync(CancellationToken cancellationToken = default);
}
