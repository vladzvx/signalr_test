using EMCCore.Models.UniversalFilters;
using IASK.EMC.Core.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("IASK.Tests.EMC")]

namespace IASK.EMC.Core.Services
{
    public class SQLRequestBuilder : IRequestBuilder
    {
        public static class Consts
        {
            public const string RequestStart = "select data||jsonb_build_object('id',type,'patient',patient_id,'header',header_id::text,'_id', id::text,'parent',parent_id::text) from {0} where";
            public const string FromParameter = "fromDate";
            public const string FromColumn = "creation_timestamp";
            public const string FromOperator = ">=";
            public const string FromPrefix = "and";

            public const string ToParameter = "toDate";
            public const string ToColumn = "creation_timestamp";
            public const string ToOperator = "<=";
            public const string ToPrefix = "and";

            public const string ProtocolsIdsParameter = "protoIds";
            public const string ProtocolsIdsColumn = "id";

            public const string PatientsIdsParameter = "patIds";
            public const string PatientsColumn = "patient_id";

            public const string DoctorsIdsParameter = "doctorIds";
            public const string DoctorsColumn = "doctor_id";

            public const string HeadersIdsParameter = "headersIds";
            public const string HeadersColumn = "header_id";

            public const string TypesIdsParameter = "typeIds";
            public const string TypeColumn = "type";

            public const string Appendix = " ORDER BY creation_timestamp DESC";

        }
        private void AddParameter(DbCommand dbCommand, string parameterName, object Value, NpgsqlTypes.NpgsqlDbType type)
        {
            if (!dbCommand.Parameters.Contains(parameterName))
            {
                dbCommand.Parameters.Add(new NpgsqlParameter(parameterName, type));
                dbCommand.Parameters[parameterName].Value = Value;
            }
            else
            {
                dbCommand.Parameters[parameterName].Value = Value;
            }
        }
        private void AddSimpleParameter(string parameterName, string columnName, string comare_operator,string prefix,object Value, DbCommand dbCommand)
        {
            if (Value == null) Value = DBNull.Value;
            dbCommand.CommandText += dbCommand.CommandText==Consts.RequestStart ? 
                string.Format(" {0} {1} @{2}", columnName, comare_operator, parameterName):
                string.Format(" {0} {1} {2} @{3}", prefix, columnName, comare_operator, parameterName);

            AddParameter(dbCommand,parameterName,Value,NpgsqlTypes.NpgsqlDbType.Timestamp);
        }
        private void AddArrayParameter(string parameterName, string columnName, IEnumerable<ulong> Value, DbCommand dbCommand, string prefix = "AND")
        {
            if (Value == null) return;
            dbCommand.CommandText += dbCommand.CommandText == Consts.RequestStart|| dbCommand.CommandText.EndsWith("(")?
                string.Format(" {0} = ANY(@{1})", columnName, parameterName) :
                string.Format(" {0} {1} = ANY(@{2})", prefix, columnName, parameterName);

            List<long> data = new List<long>();
            foreach (ulong va in Value)
            {
                data.Add((long)va);
            }
            AddParameter(dbCommand, parameterName, data, NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Bigint);
        }
        private bool CheckFilter(ICommonProtocolsFilter filter)
        {
            return filter!=null || filter.From != DateTime.MinValue || filter.To != DateTime.MinValue ||
                filter.HeadersIds != null || filter.PatientIds != null || filter.ProtocolsIds != null ||
                filter.ProtocolTypeIds != null || filter.Status != null || filter.SubjectIds != null;
        }
        public DbCommand Build(ICommonProtocolsFilter filter, DbConnection dbConnection)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandType = System.Data.CommandType.Text;
            if (!CheckFilter(filter)) throw new ArgumentException("Bad filter in request!");
            List<string> parametersNames = new List<string>();
            dbCommand.CommandText = Consts.RequestStart;
            if (filter.From != DateTime.MinValue)
            {
                AddSimpleParameter(Consts.FromParameter, Consts.FromColumn, Consts.FromOperator, Consts.FromPrefix,
                    filter.From, dbCommand);
            }

            if (filter.To != DateTime.MinValue)
            {
                AddSimpleParameter(Consts.ToParameter, Consts.ToColumn, Consts.ToOperator, Consts.ToPrefix,
                    filter.To, dbCommand);
            }

            bool hasArrays = false;
            if (dbCommand.CommandText!= Consts.RequestStart)
            {
                dbCommand.CommandText += " and";
            }
            dbCommand.CommandText += " (";
            if (filter.ProtocolsIds != null && filter.ProtocolsIds.Any())
            {
                AddArrayParameter(Consts.ProtocolsIdsParameter, Consts.ProtocolsIdsColumn, filter.ProtocolsIds.Select(item => ulong.Parse(item)), dbCommand);
                hasArrays = true;
            }

            if (filter.DoctorIds != null && filter.DoctorIds.Any())
            {
                AddArrayParameter(Consts.DoctorsIdsParameter, Consts.DoctorsColumn, filter.DoctorIds.Where(item=>item!=null).Select(item=>(ulong)item), dbCommand);
                hasArrays = true;
            }

            if (filter.PatientIds != null && filter.PatientIds.Any())
            {
                AddArrayParameter(Consts.PatientsIdsParameter, Consts.PatientsColumn, filter.PatientIds, dbCommand);
                hasArrays = true;
            }

            if (filter.HeadersIds != null && filter.HeadersIds.Any())
            {
                AddArrayParameter(Consts.HeadersIdsParameter, Consts.HeadersColumn, filter.HeadersIds.Select(item => ulong.Parse(item)), dbCommand);
                hasArrays = true;
            }

            if (filter.ProtocolTypeIds != null && filter.ProtocolTypeIds.Any())
            {
                AddArrayParameter(Consts.TypesIdsParameter, Consts.TypeColumn, filter.ProtocolTypeIds, dbCommand);
                hasArrays = true;
            }
            if (!hasArrays) dbCommand.CommandText = dbCommand.CommandText.Remove(dbCommand.CommandText.Length - 6);

            dbCommand.CommandText += hasArrays ? ") "+ Consts.Appendix + ";" : Consts.Appendix + ";";
            return dbCommand;
        }
    }
}
