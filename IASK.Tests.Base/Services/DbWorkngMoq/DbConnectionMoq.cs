using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace IASK.Tests.Base.Services.DbWorkngMoq
{
    public class DbConnectionMoq : DbConnection
    {
        public DbConnectionMoq()
        {

        }
        public override string ConnectionString { get => string.Empty; set { } }

        public override string Database => "";

        public override string DataSource => "";

        public override string ServerVersion => "moq";

        public override ConnectionState State => ConnectionState.Open;

        public override void ChangeDatabase(string databaseName)
        {
            
        }

        public override void Close()
        {

        }

        public override void Open()
        {

        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new DbTrasactionMoq();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new DbCommandMoq();
        }
    }
}
