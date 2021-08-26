using EMCCore.Models.UniversalFilters;
using IASK.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Cases.EMCReader.Models
{
    public class ProtocolRequest : BaseModel, ICommonProtocolsFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<string> ProtocolsIds { get; set; }
        public IEnumerable<ulong> PatientIds { get; set; }
        public IEnumerable<string> HeadersIds { get; set; }
        public IEnumerable<ulong> ProtocolTypeIds { get; set; }
        public IEnumerable<ulong?> DepartmentIds { get; set; }
        public IEnumerable<ulong?> DoctorIds { get; set; }
        public IEnumerable<ulong> SubjectIds { get; set; }
        public IEnumerable<ulong> ProtocolsGroupsIds { get; set; }
        public double? Status { get; set; }
    }
}
