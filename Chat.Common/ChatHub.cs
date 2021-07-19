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
        public DataHub(TestSingletone testSingletone)
        {
            int q = 0;
        }
        public async Task SendMessage(Message message)
        {
            await this.Clients.AllExcept(Context.ConnectionId).SendAsync("SendMessage", message);
        }

        public async Task Send(string message)
        {
            await this.Clients.AllExcept(Context.ConnectionId).SendAsync("send", message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }
    }
}
