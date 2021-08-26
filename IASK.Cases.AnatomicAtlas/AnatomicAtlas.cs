using Microsoft.Extensions.DependencyInjection;
using System;

namespace IASK.Cases.AnatomicAtlas
{
    public static class AnatomicAtlas
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        }
    }
}
