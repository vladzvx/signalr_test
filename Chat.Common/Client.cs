using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Common
{
    public class Client
    {
        private HubConnection connection;

        private void MessageHandler(Message message)
        {
            Console.WriteLine("{0}:{1}", message.dateTime, message.Text);
        }

        public Client(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url).AddJsonProtocol(options => {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                })
                .Build();
            connection.StartAsync().Wait();
            connection.On<Message>("SendMessage", MessageHandler);
            connection.On<Message>("Self", MessageHandler);
        }

        public void StartText()
        {
            Console.WriteLine("You can write messages here.");
            while (true)
            {
                connection.SendAsync("SendMessage", new Message() {Text = Console.ReadLine() }).Wait();
                connection.SendAsync("Self", new Message() { Text = Console.ReadLine() }).Wait();
            }
        }

    }
}
