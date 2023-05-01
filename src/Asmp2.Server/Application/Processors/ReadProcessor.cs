﻿using Asmp2.Server.Application.IO;
using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Core.Messaging;

namespace Asmp2.Server.Core.Processors;

public class ReadProcessor : IProcessor
{
    private readonly ISerialReader _serialReader;
    private readonly IMessageBroker _messageBroker;
    private readonly IMeasurementRepository _measurementRepository;

    public ReadProcessor(
        ISerialReader serialReader,
        IMessageBroker messageBroker,
        IMeasurementRepository measurementRepository
    )
    {
        _serialReader = serialReader ?? throw new ArgumentNullException(nameof(serialReader));
        _messageBroker = messageBroker ?? throw new ArgumentNullException(nameof(messageBroker));
        _measurementRepository = measurementRepository ?? throw new ArgumentNullException(nameof(measurementRepository));
    }

    public Task RunAsync(CancellationToken cancellationToken)
    {
        _serialReader.Subscribe(ProcessMessageAsync);

        return _serialReader.RunAsync(cancellationToken);
    }

    private async Task ProcessMessageAsync(Measurement measurement)
    {
        var message = new MeasurementMessage(measurement, this);
        await _measurementRepository.SaveMeasurementAsync(measurement);
        _messageBroker.Publish(message);
    }
}
