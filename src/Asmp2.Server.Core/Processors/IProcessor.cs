namespace Asmp2.Server.Core.Processors;

public interface IProcessor
{
    Task RunAsync(CancellationToken cancellationToken);
}
