using IASK.Common;
using IASK.Common.Models;
using IASK.InterviewerEngine;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.Cases.UI
{
    /// <summary>
    /// Контроллер для получения всех страниц в одном списке
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InterfaceController
    {
        private readonly IASK.InterviewerEngine.Interviewer.Factory factory;
        public InterfaceController(InterviewerEngine.Interviewer.Factory factory)
        {
            this.factory = factory;
        }

        [HttpPost()]
        [EnableCors()]
        public async Task<string> GetInterface(CheckerInput container)
        {
            CheckerOutput result = new CheckerOutput();
            try
            {
                if (container.Permit == null)
                {
                    result.SetAlert(System.Net.HttpStatusCode.Unauthorized, false, "Permit don't sended in request!");
                }
                else
                {
                    if (UMKBWorker.TryAuth(container, "0") && 
                        factory.TryGetInterviewer(IdParser.GetNewBigId(container.Lib, container.Id), out InterviewerEngine.Interviewer interviewer))
                    {
                        result.Units = interviewer.GetAllFields(container.Dialog, container.Lang);
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
