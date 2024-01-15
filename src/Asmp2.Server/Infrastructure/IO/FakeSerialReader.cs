using Asmp2.Server.Application.IO;
using Asmp2.Shared.Constants;
using System.IO.Ports;

namespace Asmp2.Server.IO;

public class FakeSerialReader : ISerialReader
{
    private readonly SerialPort _serialPort = new();
    private readonly List<Func<Measurement, Task>> subscribers = new();
    private readonly IMeasurementParser _parser;

    public FakeSerialReader(IMeasurementParser parser)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var random = new Random();

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(MilliSecondConstants.TenSeconds);

            var measurement = new Measurement(
                new Meter("fakemeter"),
                DateTime.Now,
                new PowerReading(
                    (decimal)random.NextDouble(),
                    3800m,
                    4200m
                ),
                new PowerReading(
                    (decimal)random.NextDouble(),
                    1200m,
                    2322m
                )
            );

            foreach (var subscriber in subscribers)
            {
                await subscriber(measurement);
            }
        }
    }


    public void Subscribe(Func<Measurement, Task> processMessage)
    {
        subscribers.Add(processMessage);
    }
}
