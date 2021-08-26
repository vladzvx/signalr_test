using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.Tests.Base.Services.DbWorkngMoq
{
    public class DbCommandMoq : DbCommand
    {
        public override string CommandText { get ; set ; }
        public override int CommandTimeout { get ; set ; }
        public override CommandType CommandType { get ; set ; }
        public override bool DesignTimeVisible { get ; set ; }
        public override UpdateRowSource UpdatedRowSource { get ; set ; }
        protected override DbConnection DbConnection { get ; set ; }

        private DbParameterCollectionMoq dbParameterCollection = new DbParameterCollectionMoq();
        protected override DbParameterCollection DbParameterCollection => dbParameterCollection;

        protected override DbTransaction DbTransaction { get ; set ; }

        new public DbParameterCollection Parameters { get => dbParameterCollection; }
        public override void Cancel()
        {

        }

        public override int ExecuteNonQuery()
        {
            return 0;
        }

        public override object ExecuteScalar()
        {
            return new object();
        }

        public override void Prepare()
        {

        }

        protected override DbParameter CreateDbParameter()
        {
            return null;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new DbDataReaderMoq();
        }

        public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
