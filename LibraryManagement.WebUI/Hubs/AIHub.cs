using Microsoft.AspNetCore.SignalR;

namespace learningASP.NET_CORE.Hubs
{
    public class AIHub:Hub
    {
        public async Task SendMessage(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
