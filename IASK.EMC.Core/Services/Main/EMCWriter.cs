using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using EMCCore.Interfaces;
using EMCCore.Models;
using EMCCore.Models.Patient;
using EMCCore.Models.Protocol;
using IASK.DataStorage;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services;
using IASK.DataStorage.Utils;
using IASK.EMC.Core.Models.Extentions;

namespace IASK.EMC.Core.Main
{
    public class EMCWriter :IEMCWriter
    {
        private readonly IWriterCore<Patient> patientsWriter;
        private readonly IEMCCommonWriter<EMCProtoM> protocolsWriter;
        private readonly IConnectionsFactory connectionsFactory;
        private readonly IEMCDataPreparator dataPreparator;
        public EMCWriter(IConnectionsFactory connectionsFactory, IWriterCore<Patient> patientsWriter, IEMCCommonWriter<EMCProtoM> protocolsWriter, IEMCDataPreparator dataPreparator)
        {
            this.connectionsFactory = connectionsFactory;
            this.patientsWriter = patientsWriter;
            this.protocolsWriter = protocolsWriter;
            this.dataPreparator = dataPreparator;
        }

        #region non impl

        public string WritePatient(Patient Patient)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> WritePatients(IEnumerable<Patient> Patients)
        {
            throw new NotImplementedException();
        }


        public string WriteProtcol(EMCProtoM Prococol)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> WriteProtcols(IEnumerable<EMCProtoM> Prococols)
        {
            throw new NotImplementedException();
        }
        #endregion


        public async Task<string> WritePatientAsync(Patient Patient, CancellationToken token)
        {
            using (IConnectionWrapper wrapper = await connectionsFactory.GetConnectionAsync(token))
            {
                DbCommand command = patientsWriter.CreateMainCommand(wrapper.Connection);
                await patientsWriter.ExecuteWriting(command, Patient, token);
                return Patient.id;
            }
        }

        public async Task<string> WriteProtcolAsync(EMCProtoM Prococol, CancellationToken token)
        {
            return (await WriteProtcolsAsync(new EMCProtoM[1] { Prococol }, token)).ElementAt(0);
        }


        public async Task<IEnumerable<string>> WritePatientsAsync(IEnumerable<Patient> Patients, CancellationToken token)
        {
            //MultipleDataWrapper<Patient> awaiter = new MultipleDataWrapper<Patient>(Patients);
            //patientsWriter.PutData(awaiter);
            //WritingStatus status = await awaiter.waiting;
            //if (status == WritingStatus.Writed)
            //{
            //    return Patients.Select((item) => item.id);
            //}
            //else
            //{
            //    throw new Exception("Writing was failed!");
            //}
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> WriteProtcolsAsync(IEnumerable<EMCProtoM> Prococols, CancellationToken token)
        {
            if (dataPreparator.TryPreparate(Prococols, out var messages))
            {
                var result = await protocolsWriter.Write(Prococols);
                if (result.status == EMCCore.Enums.WritingStatus.Successful)
                {
                    return Prococols.Select(item => item._id);
                }
                else
                {
                    return result.Messages;
                }
            }
            else return messages;
        }
    }
}
