using EMCCore.Models.UniversalFilters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace IASK.EMC.Core.Interfaces
{
    public interface IRequestBuilder
    {
        public DbCommand Build(ICommonProtocolsFilter filter, DbConnection dbConnection);
    }
}
