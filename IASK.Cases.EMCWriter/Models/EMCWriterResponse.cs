using IASK.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.EMCWriter.Models
{
    public class EMCWriterResponse : BaseModel
    {
        public List<string> Ids { get; set; }
    }
}
