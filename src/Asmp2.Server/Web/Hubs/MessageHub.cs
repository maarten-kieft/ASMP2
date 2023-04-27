using Microsoft.AspNetCore.SignalR;

namespace Asmp2.Server.Web.Hubs;

public class MessageHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("Message", message);
    }
}
