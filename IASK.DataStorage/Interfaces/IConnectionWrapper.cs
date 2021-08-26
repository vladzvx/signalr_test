using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace IASK.DataStorage.Interfaces
{
    public interface IConnectionWrapper : IDisposable
    {
        public DbConnection Connection { get; }
        public Guid Id { get; }
    }
}
