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
    internal static class DataHubConfigurator
    {
        internal static void ConfigureServices<T>(IServiceCollection services) where T :BaseMessage
        {
            services.AddSingleton<IKeyProvider, KeyProvider>();
            services.AddSingleton<IDataBaseSettings, DataBaseSettings>();
            services.AddSingleton<IConnectionsFactory, ConnectionsFactoryCore>();
            services.AddSingleton<Func<string, IConnectionsFactory, IConnectionWrapper>>(ConnectionWrapper.Create);

            services.AddTransient<IDbCreator<T>, DbCreator<T>>();
            services.AddTransient<IWriterCore<T>, WriterCore<T>>();
            services.AddSingleton<ICommonWriter<T>, CommonWriter<T>>();
            services.AddSingleton<DataHubState<T>>();

            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCors", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .SetIsOriginAllowed(delegate (string requestingOrigin)
                    {
                        return true;
                    }).Build();
                });
            });
        }

        internal static void Configure<T>(IApplicationBuilder app, IWebHostEnvironment env, string path) where T : BaseMessage
        {
            app.UseCors("AllowAllCors");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DataHubService<T>>(path);
                //endpoints.MapControllers();
            });
        }
    }
}
