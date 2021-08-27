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
        private void MessageHandler2(Message2 message)
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
            connection.On<Message2>("SendMessage2", MessageHandler2);
            connection.On<Message>("Self", MessageHandler);
            connection.On<Message2>("Self2", MessageHandler2);
        }

        public void StartText()
        {
            Console.WriteLine("You can write messages here.");
            while (true)
            {
                connection.SendAsync("SendMessage", new Message() {Text = "SendMessage "+ Console.ReadLine() }).Wait();
                connection.SendAsync("Self", new Message() { Text ="Self "+ Console.ReadLine() }).Wait();
                connection.SendAsync("SendMessage2", new Message2() { Text = "SendMessage2 " + Console.ReadLine() }).Wait();
                connection.SendAsync("Self2", new Message2() { Text = "Self2 " + Console.ReadLine() }).Wait();
            }
        }

    }
}
