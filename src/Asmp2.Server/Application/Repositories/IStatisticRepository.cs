namespace Asmp2.Server.Application.Repositories;

public interface IStatisticRepository
{
    Task CreateStatisticsAsync();
    IEnumerable<Statistic> GetStatistics(Period period, DateTimeOffset startDateTime);
}
