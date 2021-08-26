using IASK.Common.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewerSevice
{
    [ApiController]
    [Route("[controller]")]
    public class TestController
    {
        [HttpPost()]
        [EnableCors()]
        public async Task<string> temp(BaseModel model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
    }


}
