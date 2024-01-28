namespace Asmp2.Server.Application.Processors;

public abstract class Processor : IProcessor
{
    public Task RunAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => RunInternalAsync(cancellationToken), cancellationToken);
    }

    protected abstract Task RunInternalAsync(CancellationToken cancellationToken);
}
