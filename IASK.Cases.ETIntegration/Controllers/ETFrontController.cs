using ETALib.Requests;
using ETALib.Responses;
using ETIntegration;
using IASK.Common;
using IASK.Common.Models;
using IASK.Common.Services;
using IASK.ETIntegration.Services;
using IASK.InterviewerEngine.Models.Input;
using InterviewerSevice.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.Cases.ETIntegration.Controllers
{
    /// <summary>
    /// Контроллер для получения всех страниц в одном списке
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ETController
    {
        private readonly Retranslator retranslator;
        private readonly State state;
        public ETController(Retranslator retranslator, State state)
        {
            this.retranslator = retranslator;
            this.state = state;
        }
        #region frontend
        [HttpPost("PostStartInfo")]
        [EnableCors()]
        public async Task<string> PostStartInfo(ETALib.Requests.SubmitSymptomsRequest container)
        {
            CheckerOutput result = new CheckerOutput();
            try
            {
                if (container == null)
                {
                    result.SetAlert(System.Net.HttpStatusCode.BadRequest, false, "Request is null!");
                }
                else
                {
                    if (UMKBWorker.TryAuth(container.Authkey, "0", out Alert alert))
                    {
                        ETALib.Models.Alert alert1 = await retranslator.TryRetranslate(state.StartDataRedirectAdress, container);
                        result.SetAlert(alert1.Code.ToString(), alert1.Sticky, alert1.Message,alert1.Title,alert1.Level);
                    }
                    else result.Alert = alert;
                }
            }
            catch (Exception ex)
            {
                result.SetAlert(System.Net.HttpStatusCode.InternalServerError, sticky: false, message: ex.Message);
            }
            string temp11 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
           // state.logs.Enqueue(new Log() { Guid = container.Guid, Comment = "PostStartInfo: " + /Newtonsoft.Json.JsonConvert.SerializeObject(result.Alert) });
            return temp11;
        }

        [HttpPost("PostAnswer")]
        [EnableCors()]
        public async Task<string> PostAnswer(ETInput questionResponse)
        {
            QuestionResponse response = new QuestionResponse() {Guid = questionResponse.Guid,Finalize =questionResponse.Finalize};
            response.Questions = new System.Collections.Generic.List<ETALib.Models.Answer>();
            foreach (Answer answer in questionResponse.Answers)
            {
                if (answer.CheckerResponse!=null)
                foreach (InputStruct inputStruct in answer.CheckerResponse)
                {
                    if (ulong.TryParse(inputStruct.Id, out ulong Id))
                    {
                        response.Questions.Add(new ETALib.Models.Answer() { Id = Id, Value = Boolean.TrueString });//inputStruct.Value });
                    }
                }
            }
            CheckerOutput result = new CheckerOutput();
            ETALib.Models.Alert alert1 = await  retranslator.TryRetranslate(state.QuestionRedirectAdress, response);
            result.SetAlert(alert1.Code.ToString(), alert1.Sticky, alert1.Message,alert1.Title,alert1.Level);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [HttpPost("Feedback")]
        [EnableCors()]
        public string GetResult(ETRequest eTRequest)
        {
            CheckerOutput result = new CheckerOutput();
            if (state.TryGetResult(eTRequest.Guid, out SubmitTableRequest val))
            {
                if (val.Alert.Code == 200)
                {
                    result.Units = DataConverters.ConvertTable(val);
                }
                else
                {
                    result.SetAlert(val.Alert.Code.ToString(), val.Alert.Sticky, val.Alert.Message,val.Alert.Title, val.Alert.Level);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
            }
            else if (state.TryGetQuestion(eTRequest.Guid, out QuestionRequest que))
            {
                if (que.Alert.Code == 200)
                {
                    result.Units = DataConverters.ConvertQuestions(que, Secrets.GetNames);
                }
                else
                {
                    result.SetAlert(que.Alert.Code.ToString(), que.Alert.Sticky, que.Alert.Message, que.Alert.Title, que.Alert.Level);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
            }
            result.SetAlert(System.Net.HttpStatusCode.OK);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        #endregion

        #region for ET
        [HttpPost("PostQuestions")]
        [EnableCors()]
        public string PostQuestions(QuestionRequest questionRequest)
        {
            state.TryAdd(questionRequest.Guid,questionRequest);
            return "ok";
        }

        [HttpPost("PostResult")]
        [EnableCors()]
        public string PostResult(SubmitTableRequest submitTableRequest)
        {
            state.TryAdd(submitTableRequest.Guid, submitTableRequest);
            return "ok";
        }


        #endregion
    }
}
