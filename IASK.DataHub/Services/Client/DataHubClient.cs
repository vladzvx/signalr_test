using IASK.DataHub.Enums;
using IASK.DataHub.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.DataHub.Services
{
    public class DataHubClient<T> :IHostedService where T: BaseMessage
    {
        private HubConnection connection;

        #region handlers
        private void messageHandler(T message)
        {
            Console.WriteLine(string.Format("New! {0}", message.Text));
        }

        private void acknowledgedHandler(T message)
        {
            Console.WriteLine("acknowled! ", message);
        }

        private void updateHandler(T message)
        {
            Console.WriteLine(string.Format("Update! {0}", message.Text));
        }
        #endregion

        public DataHubClient(string url= "http://localhost:5000/wss/chat")
        {
            //url = "https://signalr-tests.ru:5002/wss/chat";
            connection = new HubConnectionBuilder()
                .WithUrl(url).AddJsonProtocol(options => {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                })
                .Build();
        }

        public void StartText(string auth)
        {
            connection.StartAsync().Wait();
            connection.On<T>("TestBroadcast", messageHandler);
            //connection.On<T>(MessageType.Acknowledged.ToString(), acknowledgedHandler);
            //connection.On<T>(MessageType.Update.ToString(), updateHandler);

            //Console.WriteLine("Write your id:");
            //connection.SendAsync("LogIn", new TestModel() { Permit = new UMKBRequests.Models.API.Codes.Permit (){authkey= auth } }).Wait();

            Console.WriteLine("You can write messages here.");
            while (true)
            {
                //Console.WriteLine("Select action: \n0 - send new message\n1 - update last message\n2 - send last message in Acknowledged");
                //var t = Console.ReadKey();
                //string Text=string.Empty;
                //MessageType messageType;
                //switch (t.KeyChar)
                //{
                //    case '0':
                //        {
                //            messageType = MessageType.New;
                //            Console.WriteLine("Write message:");
                //            Text = Console.ReadLine();
                //            break;
                //        }
                //    case '1':
                //        {
                //            messageType = MessageType.Update;
                //            Console.WriteLine("Write new text for last message:");
                //            Text = Console.ReadLine();
                //            break;
                //        }
                //    case '2':
                //        {
                //            messageType = MessageType.Acknowledged;
                //            break;
                //        }
                //    default:
                //        {
                //            messageType = MessageType.New;
                //            break;
                //        }
                //}
                connection.SendAsync("TestBroadcast", new ChatMessage() {MessageType = MessageType.New, Text= Console.ReadLine(), GroupId= 0/*(long)IdParser.GetNewBigId(123,100)*/}).Wait();
                Console.WriteLine("Write message:\n");
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await connection.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await connection.DisposeAsync();
        }
    }
}
