namespace Asmp2.Server.Core.Processors;

public interface IProcessorHost
{
    public Task RunAsync(CancellationToken cancellationToken = default);
}
