using IASK.DataHub.Models;
using IASK.DataHub.Services;
using System;

namespace client2
{
    class Program
    {
        static void Main(string[] args)
        {
            string url;
            if (args.Length == 0)
            {
                url = "https://service.socmedica.dev:9018/wss/formmailing";
                //url = "http://localhost:5000/wss/formmailing";
            }
            else
            {
                url = args[0];
            }
            DataHubClient<ChatMessage> client = new DataHubClient<ChatMessage>(url);
            client.StartText("jexVxyTKswbhRzqVMYNNMexZ");

        }
    }
}
