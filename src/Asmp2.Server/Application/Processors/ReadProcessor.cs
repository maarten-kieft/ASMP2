using Asmp2.Server.Application.IO;
using Asmp2.Server.Core.Messaging;

namespace Asmp2.Server.Core.Processors;

public class ReadProcessor : IProcessor
{
    private readonly ISerialReader _serialReader;
    private readonly IMessageBroker _messageBroker;

    public ReadProcessor(
        ISerialReader serialReader,
        IMessageBroker messageBroker
    )
    {
        _serialReader = serialReader ?? throw new ArgumentNullException(nameof(serialReader));
        _messageBroker = messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));
    }

    public Task RunAsync(CancellationToken cancellationToken)
    {
        _serialReader.Subscribe(ProcessMessage);

        return _serialReader.RunAsync(cancellationToken);
    }

    private void ProcessMessage(Measurement measurement)
    {
        var message = new MeasurementMessage(measurement, this);

        _messageBroker.Publish(message);
    }
}
