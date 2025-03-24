using Microsoft.AspNetCore.SignalR;

namespace learningASP.NET_CORE.Hubs
{
    public class AIHub:Hub
    {
       public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
