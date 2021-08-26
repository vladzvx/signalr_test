using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Common.Models
{
    public class SemanticRequest : BaseModel
    {
        public ushort Lib { get; set; }
        public uint Id { get; set; }
        public List<ulong> Result { get; set; }


    }
}
