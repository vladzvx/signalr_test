using IASK.Common.Services;
using IASK.ETIntegration;
using IASK.Cases.Checker;
using IASK.Cases.ETIntegration;
using IASK.Cases.InterviewerControllers;
using IASK.Common;
using IASK.ETIntegration.Services;
using IASK.InterviewerEngine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IASK.Cases.AnatomicAtlas;
using IASK.Cases.EMCReader;
using IASK.Cases.EMCWriter;
using IASK.Cases.JsonStorage;
using IASK.Cases.Semantic;
using IASK.Cases.DataHub;
using IASK.DataStorage.Interfaces;
using System;
using IASK.DataHub.Services;
using IASK.DataStorage.Services.Common;
using IASK.DataStorage.Services.Implementations;
using IASK.DataHub.Models;
using IASK.DataHub.Interfaces;

namespace InterviewerService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGroupsManager<ChatMessage>, DefaultGroupsManager<ChatMessage>>();
            services.AddSingleton<IKeyProvider, KeyProvider>();
            services.AddSingleton<IDataBaseSettings, DataBaseSettings>();
            services.AddSingleton<IConnectionsFactory, ConnectionsFactoryCore>();
            services.AddSingleton<Func<string, IConnectionsFactory, IConnectionWrapper>>(ConnectionWrapper.Create);

            services.AddTransient<IDbCreator<ChatMessage>, DbCreator<ChatMessage>>();
            services.AddTransient<IWriterCore<ChatMessage>, WriterCore<ChatMessage>>();
            services.AddSingleton<ICommonWriter<ChatMessage>, CommonWriter<ChatMessage>>();
            services.AddSingleton<DataHubState<ChatMessage>>();

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


            //AnatomicAtlas.ConfigureServices(services);
            //Checker.ConfigureServices(services);
            //ET.ConfigureServices(services);
            //UI.ConfigureServices(services);
            //EMCReader.ConfigureServices(services);
            //EMCWriter.ConfigureServices(services);
            //JsonStorage.ConfigureServices(services);
            //Semantic.ConfigureServices(services);
            //FormMailing.ConfigureServices(services);
            //Chat.ConfigureServices(services);
            //services.AddCors();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //FormMailing.Configure(app,env);
            app.UseCors("AllowAllCors");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DataHubService<ChatMessage>>("/chat");
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
