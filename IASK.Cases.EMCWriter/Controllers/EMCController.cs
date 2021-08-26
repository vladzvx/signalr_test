using IASK.Common.Services;
using ECPLib.Common.Patient.Models;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UMKBRequests;
using EMCCore.Interfaces;
using IASK.Common;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using IASK.Common.Models;
using EMCCore.Models.Protocol;
using IASK.Cases.EMCWriter.Models;
using IASK.EMC.Instruments;
using IASK.InterviewerEngine.Models.Input;

namespace IASK.Cases.EMCReader.Conteollers
{
    [ApiController]
    [Route("[controller]")]
    public class EMCController
    {
        private readonly IEMCDataConverter<Answer> converter;
        private readonly IEMCWriter eMCWriter;
        public EMCController(IEMCDataConverter<Answer> converter,IEMCWriter eMCWriter)
        {
            this.converter = converter;
            this.eMCWriter = eMCWriter;
        }

        [HttpPost("writeprotocol")]
        [EnableCors()]
        public async Task<string> monitoring2(EMCWriterRequest protocolRequest, CancellationToken token)
        {
            EMCWriterResponse result = new EMCWriterResponse();
            try
            {
                if (UMKBWorker.TryAuth(protocolRequest, "0"))
                {
                    if (converter.TryConvert(protocolRequest.Answers, out IEnumerable<EMCProtoM> protocols))
                    {
                        var wrRes = await eMCWriter.WriteProtcolsAsync(protocols, token);
                        result.Ids = wrRes.ToList();
                        result.SetAlert(System.Net.HttpStatusCode.OK);
                    }
                    else
                    {
                        result.SetAlert(System.Net.HttpStatusCode.InternalServerError, false, "Failed to convert data from Answers to EMCProtoM format!");
                    }
                }
                else
                {
                    result.Alert = protocolRequest.Alert;
                }
            }
            catch (Exception ex)
            {
                result.SetAlert(System.Net.HttpStatusCode.InternalServerError, sticky: false, message: ex.Message);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }

}
