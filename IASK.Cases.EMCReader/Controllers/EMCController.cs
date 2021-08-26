using IASK.Common.Services;
using ECPLib.Common.Patient.Models;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UMKBRequests;
using IASK.Cases.EMCReader.Models;
using EMCCore.Interfaces;
using IASK.Common;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using IASK.Common.Models;
using IASK.EMC.Instruments;

namespace IASK.Cases.EMCReader.Conteollers
{
    [ApiController]
    [Route("[controller]")]
    public class EMCController
    {
        private readonly IEMCDataConverter<InterfaceUnit> converter;
        private readonly IEMCReader eMCReader;
        public EMCController(IEMCDataConverter<InterfaceUnit> converter, IEMCReader eMCReader)
        {
            this.converter = converter;
            this.eMCReader = eMCReader;
        }


        [HttpPost("getprotocol")]
        [EnableCors()]
        public async Task<string> monitoring2(ProtocolRequest protocolRequest, CancellationToken token)
        {
            ReadingResultWrapper result = new ReadingResultWrapper();
            try
            {
                if (UMKBWorker.TryAuth(protocolRequest, "0"))
                {
                    var readingRes = await eMCReader.ReadProtcolAsync(protocolRequest, token);
                    if (converter.TryConvert(readingRes, out var res))
                    {
                        result.Units = res.ToList();
                        result.SetAlert(System.Net.HttpStatusCode.OK);
                    }
                    else
                    {
                        result.SetAlert(System.Net.HttpStatusCode.InternalServerError, false, "Failed to convert data from EMCProtoM to InterfaceUnit format!");
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
