using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace IASK.Tests.Base.Services.DbWorkngMoq
{
    public class DbTrasactionMoq : DbTransaction
    {
        private DbConnection dbConnection;
        public override IsolationLevel IsolationLevel => IsolationLevel.Unspecified;

        protected override DbConnection DbConnection => dbConnection;

        public override void Commit()
        {

        }

        public override void Rollback()
        {

        }
    }
}
