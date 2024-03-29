﻿using Asmp2.Server.Application.Repositories;
using Asmp2.Shared.Constants;

namespace Asmp2.Server.Application.Processors;

public class Aggregator : Processor
{
    private readonly IStatisticRepository _statisticRepository;
    private readonly IMeasurementRepository _measurementRepository;

    public Aggregator(
        IStatisticRepository statisticRepository, 
        IMeasurementRepository measurementRepository
    )
    {
        _statisticRepository = statisticRepository ?? throw new ArgumentNullException(nameof(statisticRepository));
        _measurementRepository = measurementRepository ?? throw new ArgumentNullException(nameof(measurementRepository));
    }

    protected override async Task RunInternalAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await _statisticRepository.CreateStatisticsAsync();
            await _measurementRepository.CleanMeasurementsAsync();
            await Task.Delay(MilliSecondConstants.FiveMinutes, cancellationToken);
        }
    }
}
