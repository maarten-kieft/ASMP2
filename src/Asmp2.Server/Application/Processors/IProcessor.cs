namespace Asmp2.Server.Application.Processors;

public interface IProcessor
{
    Task RunAsync(CancellationToken cancellationToken);
}
