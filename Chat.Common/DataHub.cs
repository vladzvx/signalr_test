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

    public class Message2
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
        public async Task Self(Message message)
        {
            await this.Clients.Caller.SendAsync("Self", message);
        }

        public async Task SendMessage2(Message message)
        {
            await this.Clients.All.SendAsync("SendMessage2", message);
        }
        public async Task Self2(Message message)
        {
            await this.Clients.Caller.SendAsync("Self2", message);
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
