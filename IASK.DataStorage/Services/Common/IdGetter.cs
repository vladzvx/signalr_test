using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Services.Common
{
    public class IdSource
    {
        private readonly ConcurrentBag<long> idsPool = new ConcurrentBag<long>();
        private readonly IDataBaseSettings settings;
        private readonly IConnectionsFactory connectionsFactory;

        public IdSource(IDataBaseSettings settings, IConnectionsFactory connectionsFactory)
        {
            this.connectionsFactory = connectionsFactory;
            this.settings = settings;
        }
        public long Get()
        {
            if (idsPool.Count< settings.IdPoolMinSize)
            {
                Task.Factory.StartNew(() =>
                {
                    LoadIds();
                }, TaskCreationOptions.LongRunning);
            }
            long result;
            while (!idsPool.TryTake(out result))
            {
                Thread.Sleep(100);
            }
            return result;
        }

        private void LoadIds()
        {
            using (IConnectionWrapper connectionWrapper = connectionsFactory.GetConnectionAsync(CancellationToken.None).Result)
            {
                DbCommand command = connectionWrapper.Connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "select nextval(@sec_name);";
                command.Parameters.Add(new NpgsqlParameter("sec_name",NpgsqlTypes.NpgsqlDbType.Text));
                command.Parameters["sec_name"].Value = settings.SequenceName1;
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()&& idsPool.Count<settings.IdPoolMaxSize)
                    {
                        idsPool.Add(reader.GetInt64(0));
                    }
                }
            }
            
        }
    }
}
