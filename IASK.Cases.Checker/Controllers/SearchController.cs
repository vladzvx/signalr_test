using IASK.Common;
using IASK.Common.Models;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace IASK.Controllers.InterviewerControllers
{
    /// <summary>
    /// Контроллер, ретранслирующий запросы к системе Сфински и фильтрующий результат.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Search
    {
        [HttpPost]
        [EnableCors()]
        public string SearchRequest(SearchRequest container)
        {
            SearchResult result = new SearchResult();
            try
            {
                if (container.Permit == null)
                {
                    result.SetAlert(System.Net.HttpStatusCode.Unauthorized, false, "Permit don't sended in request!");
                }
                else
                {
                    if (UMKBWorker.TryAuth(container, "0") && container.TryGetLibsString(out string LibsString))
                    {
                        List<Content> res = UMKBWorker.Search(container.Text, container.Levels, LibsString,
                            out UMKBRequests.Models.API.Semantic.Alert alert);
                        result.Alert = alert;
                        result.result = res;
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
