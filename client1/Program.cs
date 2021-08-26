﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chat.Common;
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
                //url = "https://service.socmedica.dev:9015/datahub";
                url = "http://localhost:5000/datahub";
            }
            else
            {
                url = args[0];
            }
            Client client = new Client(url);
            client.StartText();

        }
    }
}
