using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.EMC.Core.Services
{
    public class ProtocolsRotator : ActionPeriodicExecutor
    {
        private readonly IConnectionsFactory connectionsFactory;
        private double ArchivingPeriodMinuts;
        public ProtocolsRotator(IDataBaseSettings dataBaseSettings, IConnectionsFactory connectionsFactory):
            base(dataBaseSettings.DBPeriodicAction)
        {
            this.connectionsFactory = connectionsFactory;
            this.ArchivingPeriodMinuts = dataBaseSettings.ArchivingPeriodMinuts;
        }

        public override void action()
        {
            using (IConnectionWrapper wrapper = connectionsFactory.GetConnectionAsync(CancellationToken.None).Result)
            {
                DbCommand command = wrapper.Connection.CreateCommand();
                command.CommandText = "rotate_protocols";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new NpgsqlParameter("limit_time",NpgsqlTypes.NpgsqlDbType.Timestamp));
                command.Parameters["limit_time"].Value = DateTime.UtcNow.AddMinutes(-ArchivingPeriodMinuts);
                command.ExecuteNonQuery();
            }
        }
    }
}
