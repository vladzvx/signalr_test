using IASK.Common.Models;

namespace IASK.Common.Models
{
    public class RetranslatoeRequest : BaseModel
    {
        public string Url { get; set; }
        public string RequestType { get; set; }
        public string Data { get; set; }
        public string MediaType { get; set; } = "application/json";
    }
}
