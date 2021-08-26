using IASK.Common.Services;
using IASK.InterviewerEngine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.Semantic
{
    public static class Semantic
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton<IKeyProvider, KeyProvider>();
        }

    }
}
