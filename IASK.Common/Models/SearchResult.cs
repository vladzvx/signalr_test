using IASK.InterviewerEngine.Models.Output;
using System.Collections.Generic;

namespace IASK.Common.Models
{
    /// <summary>
    /// Модель с ответом от Сфинкса
    /// </summary>
    public class SearchResult : BaseModel
    {
        public List<Content> result { get; set; }
    }
}
