using IASK.InterviewerEngine.Delegates;
using System.Collections.Generic;

namespace IASK.InterviewerEngine
{
    /// <summary>
    /// Класс для хранения состояния работы вопросно-ответной системы, реализованной в <see cref="Interviewer"/>.
    /// </summary>
    public class InterviewerState
    {
        public enum ProbabilityType : ulong
        {
            /// <summary>
            /// Риск возникновения нозологии
            /// </summary>
            Risk = Constants.IDDataToRiskFactor,
            /// <summary>
            /// Вероятность наличия нозологии
            /// </summary>
            Nosology = Constants.IDDataToSymptoms
        }
        #region fields
        /// <summary>
        /// Свойство для попытки запроса списка id и названий нозологий у модуля подсчета вероятностей, 
        /// которые он способен обсчитать.
        /// </summary>
        public TryGetNosologies TryGetNosologies { get; set; }
        /// <summary>
        /// Попытка запроса вероятности нозологии у модуля подсчета вероятностей.
        /// </summary>
        public TryGetProbability TryGetProbability { get; set; }

        internal List<List<ulong>> RadiobuttonsGroups = new List<List<ulong>>();
        internal ulong CurrentQuestionTriggerID;
        internal bool CheckerEnded;
        internal DialogType DialogType = DialogType.doctor;
        internal Lang lang = Lang.ru;
        internal Dictionary<ulong, IsolationSensor> DataToIsolationSensor = new Dictionary<ulong, IsolationSensor>();
        //keys - Triggerid, values - id связей триггер-сенсор.
        internal Dictionary<ulong, List<ulong>> TriggerToSensorsLinks = new Dictionary<ulong, List<ulong>>();
        #endregion

        internal void UpdateLinks(ulong TriggerId, List<ulong> linksIds)
        {
            if (TriggerToSensorsLinks.ContainsKey(TriggerId))
            {
                TriggerToSensorsLinks[TriggerId] = linksIds;
            }
            else
            {
                TriggerToSensorsLinks.Add(TriggerId, linksIds);
            }
        }
    }
}
