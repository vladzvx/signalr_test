using IASK.ETIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IASK.Cases.ETIntegration
{
    public static class ET
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<State>();
            services.AddHttpClient<Retranslator>();
        }

    }
}
