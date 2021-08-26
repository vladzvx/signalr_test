using IASK.Common.Services;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.Checker
{
    public static class Checker
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IKeyProvider, KeyProvider>();
            services.AddSingleton<Interviewer.Factory>();
            services.AddSingleton<IProbabilityCalculatorFactory, ProbabilityCalculator.Factory>();
            services.AddTransient<InterviewerState>();
            services.AddTransient<InterviewerWrapper>();
        }
    }
}
