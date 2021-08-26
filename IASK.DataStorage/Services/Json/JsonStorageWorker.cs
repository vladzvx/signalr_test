using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services;
using IASK.DataStorage.Services.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Services.Json
{
    public class JsonStorageWorker
    {
        private readonly IConnectionsFactory connectionsFactory;

        public JsonStorageWorker(IConnectionsFactory connectionsFactory)
        {
            this.connectionsFactory = connectionsFactory;
        }

        public async Task<string> Write(string data, CancellationToken cancellationToken, long? Id=null)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException("Json value must be no null!");
            using (IConnectionWrapper connectionWrapper = await connectionsFactory.GetConnectionAsync(cancellationToken))
            {
                DbCommand command = connectionWrapper.Connection.CreateCommand();
                command.CommandText = "write_json";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new NpgsqlParameter("_data",NpgsqlTypes.NpgsqlDbType.Jsonb));
                command.Parameters["_data"].Value = data;
                if (Id != null)
                {
                    command.Parameters.Add(new NpgsqlParameter("_id", NpgsqlTypes.NpgsqlDbType.Bigint));
                    command.Parameters["_id"].Value = (long)Id;
                    await command.ExecuteNonQueryAsync(cancellationToken);
                    return ((long)Id).ToString();
                }
                else
                {
                    using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            return reader.GetInt64(0).ToString();
                        }
                    }                    
                }
            }
            throw new InvalidOperationException("smth was failed while writing json!");
        }

        public async Task<string> Read(long Id, CancellationToken cancellationToken)
        {
            using (IConnectionWrapper connectionWrapper = await connectionsFactory.GetConnectionAsync(cancellationToken))
            {
                DbCommand command = connectionWrapper.Connection.CreateCommand();
                command.CommandText = "read_json";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new NpgsqlParameter("_id", NpgsqlTypes.NpgsqlDbType.Bigint));
                command.Parameters["_id"].Value = (long)Id;
                using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        if (!reader.IsDBNull(0))
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }
            return "No jsons with such id!";
        }
    }
}
