using EMCCore.Interfaces;
using EMCCore.Models.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IASK.EMC.Core.Services.Main
{
    public class EMCDataPreparator : IEMCDataPreparator
    {
        public bool TryPreparate(IEnumerable<EMCProtoM> protocols, out IEnumerable<string> messages)
        {
            foreach (EMCProtoM proto in protocols)
            {
                proto.subproto = null;
            }
            messages = protocols.Select(item => item._id);
            return true;
        }
    }
}
