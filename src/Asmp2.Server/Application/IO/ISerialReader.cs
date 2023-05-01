namespace Asmp2.Server.Application.IO;

public interface ISerialReader
{
    Task RunAsync(CancellationToken cancellationToken);
    void Subscribe(Func<Measurement, Task> processMessage);
}
