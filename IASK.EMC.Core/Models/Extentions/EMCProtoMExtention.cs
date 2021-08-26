using EMCCore.Models.Patient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IASK.DataStorage;
using EMCCore.Models.Protocol;
using EMCCore.CommonMethods;

namespace IASK.EMC.Core.Models.Extentions
{
    public static class EMCProtoMExtension
    {
        private static EMCProtoM parentProto;
        private static EMCProtoM headerProto;

        public static void SetParent(this EMCProtoM protocol, EMCProtoM parent)
        {
            parentProto = parent;
        }

        public static void SetHeader(this EMCProtoM protocol, EMCProtoM header)
        {
            headerProto = header;
        }
        public static EMCProtoM GetParent(this EMCProtoM protocol)
        {
            return parentProto;
        }
        public static EMCProtoM GetHeader(this EMCProtoM protocol)
        {
            return headerProto;
        }


        public static bool TryParseId(this EMCProtoM protocol, out (DateTime Timestamp, long Id) res)
        {
            res = (DateTime.MinValue,0);
            if (protocol._id != null)
            {
                res = IdConverter.Convert(protocol._id);
                return true;
            }
            return false;
        }

        public static bool TryParseParentId(this EMCProtoM protocol, out (DateTime Timestamp, long Id) res)
        {
            res = (DateTime.MinValue, 0);
            if (protocol.parent != null)
            {
                res = IdConverter.Convert(protocol.parent);
                return true;
            }
            return false;
        }

        public static bool TryParseHeaderId(this EMCProtoM protocol, out (DateTime Timestamp, long Id) res)
        {
            res = (DateTime.MinValue, 0);
            if (protocol.header != null)
            {
                res = IdConverter.Convert(protocol.header);
                return true;
            }
            return false;
        }
    }
}
