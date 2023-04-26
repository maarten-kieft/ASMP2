using Asmp2.Server.Application.IO;
using Asmp2.Server.Core.Constants;
using Asmp2.Server.Core.Parsers;
using System.IO.Ports;

namespace Asmp2.Server.IO;

public class SerialReader : ISerialReader
{
    private readonly SerialPort _serialPort = new();
    private readonly List<Action<Measurement>> subscribers = new();
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

            Task.Delay(MilliSecondConstants.TenSeconds);
        }

        return Task.CompletedTask;
    }

    private Task Readasync(CancellationToken cancellationToken)
    {
        var lines = new List<string>();

        while (!cancellationToken.IsCancellationRequested)
        {
            string line = _serialPort.ReadLine();

            if (line[0] == '!')
            {
                Publish(lines);
                lines = new List<string>();
                continue;
            }

            lines.Add(line);
        }

        return Task.CompletedTask;
    }

    private void Publish(List<string> lines)
    {
        var measurement = _parser.Parse(lines);

        foreach (var subscriber in subscribers)
        {
            subscriber.Invoke(measurement);
        }
    }

    public void Subscribe(Action<Measurement> processMessage)
    {
        subscribers.Add(processMessage);
    }
}
