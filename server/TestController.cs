using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    [ApiController]
    [Route("[controller]")]
    public class TestController
    {
        public TestController()
        {
        }
        [HttpPost()]
        public async Task<string> auth()
        {
            return "ok";
        }
    }
}
