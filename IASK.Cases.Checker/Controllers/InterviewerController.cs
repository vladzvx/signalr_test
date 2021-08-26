using IASK.Common;
using IASK.Common.Models;
using IASK.InterviewerEngine;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using UMKBRequests;

namespace IASK.Cases.Checker
{
    /// <summary>
    /// Контроллер для доступа к вопросно-ответной системе
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InterviewerController
    {
        private readonly InterviewerWrapper checker;
        public InterviewerController(InterviewerWrapper checker)
        {
            this.checker = checker;
        }

        [HttpPost()]
        [EnableCors()]
        public virtual string CheckerAnswer(CheckerInput container)
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
                    if (UMKBWorker.TryAuth(container, "0"))
                    {
                        checker.Init(IdParser.GetNewBigId(container.Lib, container.Id));
                        var res = checker.SetAnsvers(container.Answers, container.Dialog, container.Lang??"ru");
                        result.Units = res.Item1;
                        result.TriggerId = res.Item2;
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
