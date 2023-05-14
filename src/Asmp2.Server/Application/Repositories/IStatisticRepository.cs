namespace Asmp2.Server.Application.Repositories;

public interface IStatisticRepository
{
    Task CreateStatisticsAsync();
    IEnumerable<Statistic> GetStatistics(string meterId, Period period, DateTimeOffset startDateTime);
}
