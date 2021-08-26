using EMCCore.Interfaces;
using EMCCore.Models.Patient;
using EMCCore.Models.Protocol;
using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services;
using IASK.DataStorage.Services.Common;
using IASK.DataStorage.Services.Implementations;
using IASK.EMC.Core.Main;
using IASK.EMC.Core.Services;
using IASK.EMC.Core.Services.Main;
using IASK.EMC.Instruments;
using IASK.InterviewerEngine;
using IASK.InterviewerEngine.Models.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IASK.Cases.EMCWriter
{
    public static class EMCWriter
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataBaseSettings, DataBaseSettings>();
            services.AddSingleton<IKeyProvider, KeyProvider>();

            services.AddSingleton<IChacheSettings, ChacheSettings>();

            services.AddSingleton<IdSource>();
            services.AddSingleton<NamesCache>();
            services.AddSingleton<LifetimeLimitedCache<ulong>>();
            services.AddSingleton<IEMCDataConverter<Answer>, CheckerAnswersConverter>();

            services.AddTransient<IWriterCore<EMCProtoM>, ProtocolsWriterCore>();
            services.AddTransient<IWriterCore<Patient>,PatientsWriterCore>();
            services.AddSingleton<IConnectionsFactory, ConnectionsFactoryCore>();
            services.AddSingleton<Func<string, IConnectionsFactory, IConnectionWrapper>>(ConnectionWrapper.Create);
            services.AddSingleton<IEMCWriter, IASK.EMC.Core.Main.EMCWriter>();
            services.AddSingleton<IEMCCommonWriter<EMCProtoM>, EMCCommonWriter<EMCProtoM>>();
            services.AddSingleton<ICommonWriter<EMCProtoM>, CommonWriter<EMCProtoM>>();
            services.AddSingleton<IEMCDataPreparator, EMCDataPreparator>();
            services.AddHostedService<ProtocolsRotator>();
            //services.AddSingleton<IIdSetter<EMCProtoM>, ProtoIdSetter>();

        }
    }
}
