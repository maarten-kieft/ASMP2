namespace Asmp2.Server.Application.Processors;

public interface IProcessorHost
{
    public IServiceProvider Services { get; }

    public Task RunAsync(CancellationToken cancellationToken = default);
}
