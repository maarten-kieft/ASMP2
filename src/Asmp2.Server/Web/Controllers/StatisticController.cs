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

    [HttpGet("{period}/{startDateTime}")]
    public IEnumerable<Statistic> Get(Period period, DateTimeOffset startDateTime)
    {
        var endDateTime = startDateTime.CalculatePeriodEndDateTime(period);
        var statistics = _statisticRepository.GetStatistics(period, startDateTime);

        return statistics;
    }
}

