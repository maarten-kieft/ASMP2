namespace Asmp2.Server.Application.Repositories;

public interface IMeterRepository
{
    Task<List<Meter>> GetMetersAsync();
}
