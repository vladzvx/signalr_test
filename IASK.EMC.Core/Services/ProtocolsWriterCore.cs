using EMCCore.Models.Protocol;
using IASK.DataStorage.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IASK.EMC.Core.Models.Extentions;

namespace IASK.EMC.Core.Services
{
    public class ProtocolsWriterCore : IWriterCore<EMCProtoM>
    {
        public DbCommand CreateMainCommand(DbConnection dbConnection)
        {
            DbCommand command = dbConnection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "add_protocol";
            command.Parameters.Add(new NpgsqlParameter("_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_type", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_creation_timestamp", NpgsqlTypes.NpgsqlDbType.Timestamp));
            command.Parameters.Add(new NpgsqlParameter("_patient_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_header_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_parent_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_doctor_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_data", NpgsqlTypes.NpgsqlDbType.Jsonb));
            return command;
        }

        public async Task ExecuteWriting(DbCommand command, EMCProtoM data, CancellationToken token)
        {
            command.Parameters["_id"].Value = long.Parse(data._id);
            command.Parameters["_type"].Value = (long)data.id;
            command.Parameters["_creation_timestamp"].Value = data.time;
            command.Parameters["_patient_id"].Value = (long)data.patient;
            if  (long.TryParse(data.header, out long header_id))
            {
                command.Parameters["_header_id"].Value = header_id;
            }
            else
            {
                command.Parameters["_header_id"].Value = 0;
            }
            if (long.TryParse(data.parent, out long parent_id))
            {
                command.Parameters["_parent_id"].Value = parent_id;
            }
            else
            {
                command.Parameters["_parent_id"].Value = 0;
            }
            command.Parameters["_doctor_id"].Value = (long)data.author;
            command.Parameters["_data"].Value = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await command.ExecuteNonQueryAsync(token);
        }
    }
}
