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

namespace InterviewerService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //AnatomicAtlas.ConfigureServices(services);
            //Checker.ConfigureServices(services);
            //ET.ConfigureServices(services);
            //UI.ConfigureServices(services);
            //EMCReader.ConfigureServices(services);
            //EMCWriter.ConfigureServices(services);
            //JsonStorage.ConfigureServices(services);
            //Semantic.ConfigureServices(services);
            //FormMailing.ConfigureServices(services);
            Chat.ConfigureServices(services);
            //services.AddCors();
            //services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //FormMailing.Configure(app,env);
            Chat.Configure(app,env);
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
            });
        }
    }
}
