using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Interfaces
{
    public interface IConnectionsFactory
    {
        public Task<IConnectionWrapper> GetConnectionAsync(CancellationToken token);

        public ConcurrentBag<IConnectionWrapper> Pool { get; }
        public ConcurrentDictionary<Guid,IConnectionWrapper> PoolRepo { get; }

    }
}
