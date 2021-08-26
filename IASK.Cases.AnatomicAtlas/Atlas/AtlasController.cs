using IASK.Common;
using IASK.Common.Models;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UMKBRequests;

namespace InterviewerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AtlasController
    {
        [HttpPost("getsymptoms")]
        [EnableCors()]
        public string GetSymptoms(CheckerInput container /*RequestCheckNosologyAnswer request*/)
        {
            CheckerOutput result = new CheckerOutput();
            result.Units = new List<InterfaceUnit>();
            try
            {
                if (container.Permit == null)
                {
                    result.SetAlert(System.Net.HttpStatusCode.Unauthorized, false, "Permit don't sended in request!");
                }
                else
                {
                    if (UMKBWorker.TryAuth(container, "0"))
                    {
                        ulong CheckerId = IdParser.GetNewBigId(container.Lib, container.Id);
                        if (UMKBWorker.TryGetSemantic(CheckerId,Secrets.Semantic, out InterfaceUnit iu))
                        {
                            result.Units.Add(iu);
                        }
                        result.Alert.Ok();
                    }
                    else result.Alert = container.Alert;
                }
            }
            catch (Exception ex)
            {
                result.SetAlert(System.Net.HttpStatusCode.InternalServerError, sticky: false, message: ex.Message);
            }
            string temp11 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return temp11;
        }
    }

}
