using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Interfaces
{
    public interface IWriterCore<T>
    {
        public DbCommand CreateMainCommand(DbConnection dbConnection);
        public Task ExecuteWriting(DbCommand command, T data, CancellationToken token);
    }
}
