using IASK.DataHub.Models;
using IASK.DataStorage.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataHub.Services
{
    public class WriterCore<T> : IWriterCore<T> where T: BaseMessage
    {
        public DbCommand CreateMainCommand(DbConnection dbConnection)
        {
            DbCommand command = dbConnection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "write_message_"+IDbCreator<T>.Name;
            command.Parameters.Add(new NpgsqlParameter("_message_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_group_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_time", NpgsqlTypes.NpgsqlDbType.Timestamp));
            command.Parameters.Add(new NpgsqlParameter("_user_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_data", NpgsqlTypes.NpgsqlDbType.Jsonb));
            return command;
        }

        public async Task ExecuteWriting(DbCommand command, T data, CancellationToken token)
        {
            command.Parameters["_message_id"].Value =data.Id;
            command.Parameters["_group_id"].Value =data.GroupId;
            command.Parameters["_time"].Value =data.DateTime;
            command.Parameters["_user_id"].Value =data.UserId;
            command.Parameters["_data"].Value = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await command.ExecuteNonQueryAsync(token);
        }

    }
}
