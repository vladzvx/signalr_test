using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.InterviewerControllers
{
    public static class UI
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Interviewer.Factory>();
            services.AddSingleton<IProbabilityCalculatorFactory, ProbabilityCalculator.Factory>();
            services.AddTransient<InterviewerState>();
            services.AddTransient<InterviewerWrapper>();
        }
    }
}
