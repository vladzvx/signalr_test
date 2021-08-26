using EMCCore.Interfaces;
using EMCCore.Models.Patient;
using EMCCore.Models.Protocol;
using EMCCore.Models.UniversalFilters;
using IASK.DataStorage.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMCCore.CommonMethods;
using IASK.DataStorage.Interfaces;
using System.Data.Common;
using IASK.EMC.Core.Interfaces;
using IASK.EMC.Instruments.Services.Converters;

namespace IASK.EMC.Core.Main
{
    public class EMCReader : IEMCReader
    {
        private readonly IConnectionsFactory connectionsFactory;
        private readonly IRequestBuilder requestBuilder;
        private readonly GroupsRepo groupsRepo;
        public EMCReader(IConnectionsFactory connectionsFactory, IRequestBuilder requestBuilder, GroupsRepo groupsRepo)
        {
            this.connectionsFactory = connectionsFactory;
            this.requestBuilder = requestBuilder;
            this.groupsRepo = groupsRepo;
        }

        #region non impl
        public IEnumerable<Patient> ReadPatients(ICommonPatientsFilter Filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EMCProtoM> ReadProtcol(ICommonProtocolsFilter Filter)
        {
            throw new NotImplementedException();
        }
        

        public async Task<IEnumerable<Patient>> ReadPatientsAsync(ICommonPatientsFilter Filter, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        #endregion

        public async Task<IEnumerable<EMCProtoM>> ReadProtcolAsync(ICommonProtocolsFilter Filter, CancellationToken token)
        {
            if (Filter.ProtocolsGroupsIds != null && Filter.ProtocolsGroupsIds.Any())
            {
                HashSet<ulong> Ids = new HashSet<ulong>();
                if (Filter.ProtocolTypeIds != null)
                {
                    foreach (ulong id in Filter.ProtocolTypeIds)
                    {
                        Ids.Add(id);
                    }
                }
                foreach (ulong group in Filter.ProtocolsGroupsIds)
                {
                    IEnumerable<ulong> types = await groupsRepo.GetGroupMembers(group, token);
                    foreach (ulong type in types)
                    {
                        Ids.Add(type);
                    }
                }
                Filter.ProtocolTypeIds = Ids;
            }

            List<EMCProtoM> result = new List<EMCProtoM>();
            using (IConnectionWrapper wrapper = await connectionsFactory.GetConnectionAsync(token))
            {
                DbCommand command = requestBuilder.Build(Filter, wrapper.Connection);
                string commandTextReserve = command.CommandText;
                command.CommandText = string.Format(commandTextReserve, "protocolscache");
                using (DbDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    while (await reader.ReadAsync())
                    {
                        string data = reader.GetString(0);
                        EMCProtoM currProt = Newtonsoft.Json.JsonConvert.DeserializeObject<EMCProtoM>(data);
                        result.Add(currProt);
                    }
                }
                command.CommandText = string.Format(commandTextReserve, "protocols");
                using (DbDataReader reader = await command.ExecuteReaderAsync(token))
                {
                    while (await reader.ReadAsync())
                    {
                        string data = reader.GetString(0);
                        EMCProtoM currProt = Newtonsoft.Json.JsonConvert.DeserializeObject<EMCProtoM>(data);
                        result.Add(currProt);
                    }
                }
                return result;
            }
        }
    }
}
