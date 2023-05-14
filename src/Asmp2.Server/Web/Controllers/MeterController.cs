using Asmp2.Server.Application.Repositories;

namespace Asmp2.Server.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MeterController : ControllerBase
{
    private readonly IMeterRepository _meterRepository;

    public MeterController(IMeterRepository meterRepository)
    {
        _meterRepository = meterRepository ?? throw new ArgumentNullException(nameof(meterRepository));
    }

    [HttpGet("all")]
    public async Task<List<Meter>> Get()
    {
        var meters = await _meterRepository.GetMetersAsync();

        return meters;
    }
}

