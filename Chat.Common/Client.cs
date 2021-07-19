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

        //private void MessageHandler(Message message)
        //{
        //    Console.WriteLine("{0}:{1}",message.dateTime, message.Text);
        //}

        private void stringHandler(string message)
        {
            Console.WriteLine(message);
        }
        public Client(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url).AddJsonProtocol(options => {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                })
                .Build();
            connection.StartAsync().Wait();
            //   connection.On<Message>("SendMessage", MessageHandler);
            connection.On<string>("send", stringHandler);
        }

        //public void Start()
        //{
        //    Console.WriteLine("You can write messages here.");
        //    while (true)
        //    {
        // /       Message message = new Message() { dateTime = DateTime.UtcNow, Text = Console.ReadLine() };
        //        connection.SendAsync("SendMessage", message).Wait();
        //    }
        //}

        public void StartText()
        {
            Console.WriteLine("You can write messages here.");
            while (true)
            {
                connection.SendAsync("send", Console.ReadLine()).Wait();
            }
        }

    }
}
