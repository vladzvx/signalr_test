using IASK.InterviewerEngine.Models.Output;
using System.Collections.Generic;

namespace IASK.Common.Models
{
    /// <summary>
    /// Сформированный экран для фронтэнда
    /// </summary>
    public class CheckerOutput : BaseModel
    {
        /// <summary>
        /// ID текущего триггера (вопроса)
        /// </summary>
        public string TriggerId { get; set; }
        /// <summary>
        /// Элементы интерфейса
        /// </summary>
        public List<InterfaceUnit> Units { get; set; }
    }
}
