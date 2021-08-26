using IASK.Common.Services;
using IASK.DataHub.Interfaces;
using IASK.DataHub.Models;
using IASK.DataHub.Services;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services.Common;
using IASK.DataStorage.Services.Implementations;
using IASK.InterviewerEngine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;


namespace IASK.Cases.DataHub
{
    public static class FormMailing
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            DataHubConfigurator.ConfigureServices<FormMessage>(services);
            services.AddSingleton<IGroupsManager<FormMessage>, InputFormGroupsManager>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DataHubConfigurator.Configure<FormMessage>(app, env, "/wss/formmailing");
        }

    }
}
