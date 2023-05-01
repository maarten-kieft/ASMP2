using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Core.Processors;
using Asmp2.Shared.Constants;

namespace Asmp2.Server.Application.Processors;

public class Aggregator : IProcessor
{
    private readonly IStatisticRepository _statisticRepository;

    public Aggregator(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository ?? throw new ArgumentNullException(nameof(statisticRepository));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await _statisticRepository.CreateStatisticsAsync();
            await Task.Delay(MilliSecondConstants.FiveMinutes);
        }
    }
}
