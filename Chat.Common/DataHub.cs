using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Common
{

    public class Message
    {
        public DateTime dateTime { get; set; } = DateTime.UtcNow;
        public string Text { get; set; }
    }
    public class DataHub : Hub
    {
        public DataHub()
        {

        }
        public async Task SendMessage(Message message)
        {
            await this.Clients.All.SendAsync("SendMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
