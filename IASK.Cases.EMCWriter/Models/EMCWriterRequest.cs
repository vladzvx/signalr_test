using IASK.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.EMCWriter.Models
{
    public class EMCWriterRequest : CheckerInput
    {
        public ulong PatientId { get; set; }
    }
}
