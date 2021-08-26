using EMCCore.Interfaces;
using EMCCore.Models;
using EMCCore.Models.Protocol;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Services.Common;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.EMC.Instruments
{
    public class CheckerAnswersConverter : IEMCDataConverter<Answer>
    {
        private readonly IdSource idSource;
        private double MutableShift;
        public CheckerAnswersConverter(IdSource idSource, IDataBaseSettings settings)
        {
            this.idSource = idSource;
            MutableShift = settings.ArchivingPeriodMinuts;
        }
        public bool TryConvert(IEnumerable<Answer> data, out IEnumerable<EMCProtoM> protocols)
        {
            try
            {
                protocols = ConvertAnswers(data.ToList());
                return true;
            }
            catch (Exception ex)
            {
                protocols = null;
                return false;
            }
        }

        [Obsolete("Not implemeted! Throws NotImplementedException")]
        public bool TryConvert(IEnumerable<EMCProtoM> protocols, out IEnumerable<Answer> data)
        {
            throw new NotImplementedException();
        }

        private List<EMCProtoM> ConvertAnswers(List<Answer> answers)
        {
            Dictionary<ulong, List<Answer>> sortedAnswers = new Dictionary<ulong, List<Answer>>();
            Dictionary<ulong, ulong> patients = new Dictionary<ulong, ulong>();
            foreach (Answer ans in answers)
            {
                if (ulong.TryParse(ans.TriggerId,out ulong triggerId))
                {
                    if (sortedAnswers.ContainsKey(triggerId))
                    {
                        if (patients[triggerId]!= ans.PatientId)
                        {
                            throw new ArgumentException("Data from single trigger must belongs to single patient!");
                        }
                        sortedAnswers[triggerId].Add(ans);
                    }
                    else
                    {
                        sortedAnswers.Add(triggerId, new List<Answer>() { ans });
                        patients[triggerId] = ans.PatientId;
                    }
                }
            }
            List<EMCProtoM> result = new List<EMCProtoM>();

            foreach (ulong key in sortedAnswers.Keys)
            {
                EMCProtoM header = new EMCProtoM();
                header.time = DateTime.UtcNow;
                header.patient = sortedAnswers[key][0].PatientId;
                header.id = key;
                header._id = idSource.Get().ToString(); //ObjectId.GenerateNewId().ToString();
                header.author= sortedAnswers[key][0].DoctorId;
                bool headerAdded = false;
                foreach (Answer answer in sortedAnswers[key])
                {
                    foreach (InputStruct inputStruct in answer.CheckerResponse)
                    {
                        EMCProtoM current = new EMCProtoM();
                        current.id = ulong.Parse(inputStruct.Idb);
                        if (ulong.TryParse(inputStruct.EMCId,out _))
                        {
                            current._id = inputStruct.EMCId;
                            current.time = DateTime.UtcNow.AddMinutes(-MutableShift*2);
                        }
                        else
                        {
                            current._id = idSource.Get().ToString();
                            if (!headerAdded) result.Add(header);
                            current.time = DateTime.UtcNow;
                        }

                        current.patient = answer.PatientId;
                        current.author = answer.DoctorId;
                        current.header = header._id;
                        current.parent = header._id;
                        current.values = new Dictionary<string, List<string>>();
                        current.values.Add("FreeText", new List<string>() { inputStruct.Value });
                        result.Add(current);
                    }
                }
            }
            return result;
        }
    }
}
