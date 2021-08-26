using IASK.Common;
using IASK.Common.Models;
using IASK.InterviewerEngine;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.Cases.Semantic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemanticController
    {
        private readonly IKeyProvider keyProvider;
        public SemanticController(IKeyProvider keyProvider)
        {
            this.keyProvider = keyProvider;
        }

        public async Task<string> GetSmth(SemanticRequest container, ushort lib, ushort level, short route)
        {
            try
            {
                if (await UMKBWorker.TryAuth(container))
                {
                    var result = await UMKBWorker.GetSimpleSemanticAsync(IdParser.GetNewBigId(container.Lib,container.Id), lib, level, keyProvider.SemanticKey, route);
                    container.Result = result;
                }
            }
            catch (Exception ex)
            {
                container.SetAlert(ex);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(container);
        }

        [HttpPost("patients")]
        [EnableCors()]
        public async Task<string> GetPatients(SemanticRequest container)
        {
            return await GetSmth(container, 119, 9113, 0);
        }


        [HttpPost("doctors")]
        [EnableCors()]
        public async Task<string> GetDoctors(SemanticRequest container)
        {
            return await GetSmth(container, 120, 9113, 1);
        }
    }
}
