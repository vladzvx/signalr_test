using ETIntegration;
using IASK.Common;
using IASK.Common.Models;
using IASK.Common.Services;
using IASK.ETIntegration.Services;
using InterviewerSevice.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.Cases.ETIntegration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RetranslatorController
    {
        private readonly Retranslator retranslator;
        public RetranslatorController(Retranslator retranslator)
        {
            this.retranslator = retranslator;
        }

        [HttpPost("res")]
        [EnableCors()]
        public async Task<string> action(RetranslatoeRequest req, CancellationToken cancellationToken)
        {
            BaseModel result = new BaseModel();
            try
            {
                if (req.Permit == null)
                {
                    req.SetAlert(System.Net.HttpStatusCode.Unauthorized, false, "Permit don't sended in request!");
                }
                else
                {
                    if (UMKBWorker.TryAuth(req, "0"))
                    {
                        return await retranslator.Retranslate(req.Url, req.Data, req.RequestType, cancellationToken, req.MediaType);
                    }
                    else result.Alert = result.Alert;
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
