using IASK.DataStorage.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace IASK.DataStorage.Services.Common
{
    public class ConnectionWrapper : IConnectionWrapper
    {
        private readonly IConnectionsFactory connectionPoolManager;
        public DbConnection Connection { get; private set; }
        public Guid Id { get; private set; }
        internal ConnectionWrapper(string ConnectionString, IConnectionsFactory connectionPoolManager)
        {
            this.connectionPoolManager = connectionPoolManager;
            Id = Guid.NewGuid();
        }

        public static IConnectionWrapper Create(string ConnectionString, IConnectionsFactory connectionPoolManager)
        {
            var temp = new ConnectionWrapper(ConnectionString, connectionPoolManager);
            temp.Connection = new NpgsqlConnection();
            temp.Connection.ConnectionString = ConnectionString;
            temp.Connection.Disposed += temp.Connection_Disposed1;
            return temp;
        }


        private void Connection_Disposed1(object sender, EventArgs e)
        {
            connectionPoolManager.PoolRepo.TryRemove(Id, out var _);
        }
        public void Dispose()
        {
            connectionPoolManager.Pool.Add(this);
        }
    }
}
