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
    public class ConnectionsFactoryCore : IConnectionsFactory 
    {
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private readonly IDataBaseSettings settings;
        private readonly Func<string, IConnectionsFactory, IConnectionWrapper> GetWrapper;
        public ConcurrentBag<IConnectionWrapper> Pool { get; } = new ConcurrentBag<IConnectionWrapper>();
        public ConcurrentDictionary<Guid, IConnectionWrapper> PoolRepo { get; } = new ConcurrentDictionary<Guid, IConnectionWrapper>();
        private readonly object locker = new object();
        public int TotalConnections
        {
            get { return PoolRepo.Count; }
        }
        public int HotReserve
        {
            get { return Pool.Count; }
        }

        public ConnectionsFactoryCore(IDataBaseSettings settings, Func<string, IConnectionsFactory, IConnectionWrapper> GetWrapper)
        {
            this.GetWrapper = GetWrapper;
            this.settings = settings;
            timer.Interval = settings.PoolingTimeout;
            ConnectionsManaging(null, null);
            timer.Elapsed += ConnectionsManaging;
            timer.AutoReset = true;
            timer.Start();
        }
        private void ConnectionsManaging(object sender, System.Timers.ElapsedEventArgs args)
        {
            while (Pool.Count > settings.ConnectionPoolHotReserve &&
                Pool.TryTake(out IConnectionWrapper connection) &&
                PoolRepo.TryRemove(connection.Id, out _))
            {
                try
                {
                    connection.Connection.Close();
                    connection.Connection.Dispose();
                }
                catch { }
            }
        }
        private async Task<IConnectionWrapper> CreateConnectionAsync(CancellationToken token)
        {
            IConnectionWrapper connection = GetWrapper(settings.ConnectionString1,this);
            await connection.Connection.OpenAsync(token);
            PoolRepo.TryAdd(connection.Id, connection);
            return connection;
        }
        public async Task<IConnectionWrapper> GetConnectionAsync(CancellationToken token)
        {
            IConnectionWrapper connection = null;
            while (!token.IsCancellationRequested)
            {
                if (Pool.TryTake(out connection))
                {
                    if (connection.Connection.State == System.Data.ConnectionState.Open)
                        return connection;
                    else if (connection.Connection.State == System.Data.ConnectionState.Connecting)
                    {
                        continue;
                    }
                    else
                    {
                        PoolRepo.TryRemove(connection.Id, out var _);
                    }
                }
                else if (PoolRepo.Count < this.settings.ConnectionPoolMaxSize)
                {
                    try
                    {
                        connection = await CreateConnectionAsync(token);
                        return connection;
                    }
                    catch (Exception ex)
                    {
                        // Monitor.Exit(locker);
                    }
                }
                else await Task.Delay(100, token);
            }
            throw new OperationCanceledException();
        }
    }
}
