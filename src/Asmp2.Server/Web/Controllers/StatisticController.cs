 using Asmp2.Server.Application.Repositories;
using Asmp2.Server.Core.Extensions;

namespace Asmp2.Server.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticController : ControllerBase
{
    private readonly IStatisticRepository _statisticRepository;

    public StatisticController(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository ?? throw new ArgumentNullException(nameof(statisticRepository));
    }

    [HttpGet("{meterId}/{period}/{startDateTime}")]
    public IEnumerable<Statistic> Get(string meterId, Period period, DateTimeOffset startDateTime)
    {
        var statistics = _statisticRepository.GetStatistics(meterId, period, startDateTime);

        return statistics;
    }
}

