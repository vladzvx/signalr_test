using IASK.Common.Services;
using ECPLib.Common.Patient.Models;
using EMCLib.EMC.Requests;
using IASK.InterviewerEngine.Models.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UMKBRequests;
using IASK.Cases.EMCIntegration.Models;

namespace IASK.Cases.EMCIntegration
{

    [ApiController]
    [Route("[EMC]")]
    public class EMCReadController
    {
        private readonly DataConverter converter;
        public EMCReadController(DataConverter converter)
        {
            this.converter = converter;
        }

        [HttpPost("getprotocol")]
        [EnableCors()]
        public string monitoring2()
        {
            List<EMCProtoM> data1 = new List<EMCProtoM>();
            List<List<EMCProtoM>>  data2 = Person1.CreateRandomVacinationProfile(0, new DateTime(2000, 11, 4));
            foreach (var d in data2)
            {
                data1.AddRange(d);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Cont() { Units = converter.ConvertProtocols(data1)});
        }
    }

}
