using Microsoft.AspNetCore.SignalR;

namespace Asmp2.Server.Web.Hubs;

public class MeasurementHub : Hub
{
    public async Task SendMessage(Measurement measurement)
    {
        await Clients.All.SendAsync("Measurement", measurement);
    }
}
