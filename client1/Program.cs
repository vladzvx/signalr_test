using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IASK.DataHub.Models;
using IASK.DataHub.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace client1
{
    class Program
    {
        static void Main(string[] args)
        {
            string url;
            if (args.Length == 0)
            {
                //url = "https://service.socmedica.dev:9018/wss/formmailing";
                url = "https://signalr-tests.ru:5002/wss/chat";
                //url = "http://localhost:5000/wss/formmailing";
            }
            else
            {
                url = args[0];
            }
            DataHubClient<ChatMessage> client = new DataHubClient<ChatMessage>(url);
            client.StartText("BGGKxviyBLdmiXaGY0BZUGt0");

        }
    }
}
