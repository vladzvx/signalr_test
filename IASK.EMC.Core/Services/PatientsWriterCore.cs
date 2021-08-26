using EMCCore.Models.Patient;
using IASK.DataStorage.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.EMC.Core.Services
{
    public class PatientsWriterCore : IWriterCore<Patient>
    {
        public DbCommand CreateMainCommand(DbConnection dbConnection)
        {
            DbCommand command = dbConnection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "add_patient";
            command.Parameters.Add(new NpgsqlParameter("_birthdate", NpgsqlTypes.NpgsqlDbType.Timestamp));
            command.Parameters.Add(new NpgsqlParameter("_id", NpgsqlTypes.NpgsqlDbType.Bigint));
            command.Parameters.Add(new NpgsqlParameter("_main_address", NpgsqlTypes.NpgsqlDbType.Text));
            command.Parameters.Add(new NpgsqlParameter("_data", NpgsqlTypes.NpgsqlDbType.Jsonb));
            return command;
        }

        public async Task ExecuteWriting(DbCommand command, Patient data, CancellationToken token)
        {
            object id;
            if (data.id != null) id = data.id;
            else id = DBNull.Value;

            object birthdate;
            if (data.birthdate != null) birthdate = new DateTime((long)data.birthdate);
            else birthdate = DBNull.Value;

            object main_address;
            if (data.main_address != null) main_address = data.main_address;
            else main_address = DBNull.Value;

            command.Parameters["_birthdate"].Value = birthdate;
            command.Parameters["_id"].Value = id;
            command.Parameters["_main_address"].Value = main_address;
            command.Parameters["_data"].Value = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await command.ExecuteNonQueryAsync(token);
        }
    }
}
