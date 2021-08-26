using EMCCore.Interfaces;
using IASK.Cases.EMCReader.Services;
using IASK.Common.Interfaces;
using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services;
using IASK.DataStorage.Services.Common;
using IASK.DataStorage.Services.Implementations;
using IASK.EMC.Core.Interfaces;
using IASK.EMC.Core.Services;
using IASK.EMC.Instruments;
using IASK.EMC.Instruments.Services.Converters;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.EMCReader
{
    public static class EMCReader
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IKeyProvider, KeyProvider>();
            services.AddSingleton<IDataBaseSettings, DataBaseSettings>();

            services.AddSingleton<IChacheSettings, ChacheSettings>();
            services.AddSingleton<HttpClientWrapper>();
            services.AddSingleton<IUrlFactory, UrlFactory>();
            services.AddSingleton<NamesCache>();
            services.AddSingleton<LifetimeLimitedCache<ulong>>();

            services.AddSingleton<IRequestBuilder, SQLRequestBuilder>();
            services.AddSingleton<IConnectionsFactory,ConnectionsFactoryCore>();
            services.AddSingleton<Func<string, IConnectionsFactory, IConnectionWrapper>>(ConnectionWrapper.Create);
            services.AddSingleton<IEMCDataConverter<InterfaceUnit>, InterfaceUnitConverter>();
            services.AddSingleton<IEMCReader,IASK.EMC.Core.Main.EMCReader>();

            services.AddSingleton<GroupsRepo>();
            services.AddHostedService<Starter>();
        }
    }
}
