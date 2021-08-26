using IASK.Common.Services;
using IASK.Common.Services;
using IASK.InterviewerEngine;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IASK.Cases.EMCIntegration
{
    public static class EMC
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<DataConverter>();
            services.AddTransient<IKeyProvider, KeyProvider>();
            services.AddTransient<IChacheSettings, ChacheSettings>();
            services.AddScoped<NamesCache>();
            services.AddSingleton<LifetimeLimitedCache<ulong>>();
        }
    }
}
