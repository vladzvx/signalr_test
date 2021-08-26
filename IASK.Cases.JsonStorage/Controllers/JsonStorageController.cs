using IASK.Common;
using IASK.Common.Models;
using IASK.DataStorage.Services;
using IASK.Cases.JsonStorage.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using IASK.DataStorage.Services.Json;
using IASK.Cases.JsonStorage.Utils;

namespace IASK.Cases.JsonStorage
{
    [ApiController]
    [Route("[controller]")]
    public class JsonStorageController:ControllerBase
    {
        private readonly JsonStorageWorker jsonStorageWorker;
        private readonly JsonProcessor jsonProcessor;
        public JsonStorageController(JsonStorageWorker jsonStorageWorker, JsonProcessor jsonProcessor)
        {
            this.jsonStorageWorker = jsonStorageWorker;
            this.jsonProcessor = jsonProcessor;
        }

        [HttpPost("write")]
        [EnableCors()]
        public async Task<string> write(CancellationToken cancellationToken)
        {
            BaseModel result = new BaseModel();
            try
            {
                using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string data = await reader.ReadToEndAsync();
                    var authData = jsonProcessor.GetAuthData(data);
                    long? Id = null;
                    if (jsonProcessor.TryGetId(data, out long id))
                    {
                        Id = id;
                    }
                    if (UMKBWorker.TryAuth(authData.AuthKey, authData.Service, out var alert))
                    {
                        return await jsonStorageWorker.Write(jsonProcessor.GetDataJson(jsonProcessor.GetData(data)), cancellationToken, Id);
                    }
                    else result.SetAlert(alert);
                }
            }
            catch (Exception ex)
            {
                result.SetAlert(System.Net.HttpStatusCode.InternalServerError, sticky: false, message: ex.Message);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        [HttpPost("read")]
        [EnableCors()]
        public async Task<string> read(JsonRequest jsonRequest,CancellationToken cancellationToken)
        {
            BaseModel result = new BaseModel();
            try
            {
                if (UMKBWorker.TryAuth(jsonRequest, "0"))
                {
                    return await jsonStorageWorker.Read(jsonRequest.JsonId, cancellationToken);
                }
                else
                {
                    result.SetAlert(System.Net.HttpStatusCode.Unauthorized);
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
