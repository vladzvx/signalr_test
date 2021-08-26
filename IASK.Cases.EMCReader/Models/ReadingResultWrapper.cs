using IASK.Common.Models;
using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.EMCReader.Models
{
    public class ReadingResultWrapper : BaseModel
    {
        public List<InterfaceUnit> Units { get; set; }
    }
}
