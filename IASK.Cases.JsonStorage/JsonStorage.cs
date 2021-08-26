using IASK.Cases.JsonStorage.Utils;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services;
using IASK.DataStorage.Services.Common;
using IASK.DataStorage.Services.Implementations;
using IASK.DataStorage.Services.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.JsonStorage
{
    public static class JsonStorage
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<JsonProcessor>();
            services.AddTransient<JsonStorageWorker>();
            services.AddSingleton<IConnectionsFactory,ConnectionsFactoryCore>();
            services.AddSingleton<Func<string, IConnectionsFactory, IConnectionWrapper>>(ConnectionWrapper.Create);
            services.AddSingleton<IDataBaseSettings, DataBaseSettings>();
        }
    }
}
