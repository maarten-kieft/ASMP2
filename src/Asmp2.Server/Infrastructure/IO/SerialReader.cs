using Asmp2.Server.Application.IO;
using Asmp2.Server.Core.Parsers;
using Asmp2.Shared.Constants;
using System.IO.Ports;

namespace Asmp2.Server.IO;

public class SerialReader : ISerialReader
{
    private readonly SerialPort _serialPort = new();
    private readonly List<Func<Measurement, Task>> subscribers = new();
    private readonly IMeasurementParser _parser;

    public SerialReader(IMeasurementParser parser)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await Connect(cancellationToken);
        await Readasync(cancellationToken);
    }

    private Task Connect(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var portNames = SerialPort.GetPortNames();

            foreach (var portName in portNames)
            {
                _serialPort.PortName = portName;
                _serialPort.BaudRate = 115200;
                _serialPort.Open();

                return Task.CompletedTask;
            }

            Task.Delay(MilliSecondConstants.TenSeconds, cancellationToken);
        }

        return Task.CompletedTask;
    }

    private async Task Readasync(CancellationToken cancellationToken)
    {
        var lines = new List<string>();

        while (!cancellationToken.IsCancellationRequested)
        {
            string line = _serialPort.ReadLine();

            if (line[0] == '!')
            {
                await PublishAsync(lines);
                lines = new List<string>();
                continue;
            }

            lines.Add(line);
        }
    }

    private async Task PublishAsync(List<string> lines)
    {
        var measurement = _parser.Parse(lines);

        foreach (var subscriber in subscribers)
        {
            await subscriber(measurement);
        }
    }

    public void Subscribe(Func<Measurement, Task> processMessage)
    {
        subscribers.Add(processMessage);
    }
}
