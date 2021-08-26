using IASK.InterviewerEngine.Interfaces;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;

namespace IASK.InterviewerEngine
{
    /// <summary>
    /// Реализация чекера: <see cref="Interviewer"/> + элементы электронного терапевта, 
    /// взятые из реализации чекера в проекте https://gitlab.socmedica.dev/root/globaldev
    /// </summary>
    public class InterviewerWrapper
    {
        /// <summary>
        /// Структура для передачи в чекер ключей доступа к UMKB
        /// </summary>
        public struct RequestsKeys
        {
            public string GraphPass;
            public string DescriptionsList;
            public string VarcharList;
            public string BooleanKey;
            public string NamesKey;
        }
        /// <summary>
        /// id чекера
        /// </summary>
        public ulong CheckerId { get; private set; } = 0;
        internal IProbabilityCalculator probabilityCalculator;
        internal Interviewer interviewer;
        private readonly Interviewer.Factory interviewerFactory;
        private readonly IProbabilityCalculatorFactory calcFactory;
        public InterviewerWrapper(Interviewer.Factory interviewerFactory, IProbabilityCalculatorFactory calcFactory)
        {
            this.interviewerFactory = interviewerFactory;
            this.calcFactory = calcFactory;
        }

        /// <summary>
        /// Загрузка данных из UMKB и создание чекера
        /// </summary>
        public void Init(ulong CheckerId)
        {
            this.CheckerId = CheckerId;
            if (interviewerFactory.TryGetInterviewer(CheckerId, out interviewer))
            {
                if (interviewer.Type == InterviewerType.Checker)
                    calcFactory.TryGetProbabilityCalculator(CheckerId, out probabilityCalculator);
            }
            else
            {
                throw new Exception("InterviewerWrapper init failed");
            }
        }

        /// <summary>
        /// Основной метод для взаимодействия с чекером.
        /// </summary>
        /// <param name="answers"></param>
        /// <param name="dialogType"></param>
        /// <returns></returns>
        public (List<InterfaceUnit>, string) SetAnsvers(List<Answer> answers, string dialogType = "doctor", string lang="ru")
        {
            IProbabilityCalculator temp = null; 
            InterviewerState state = new InterviewerState();
            if (probabilityCalculator != null)
            {
                temp = probabilityCalculator.Copy();
                state.TryGetProbability = temp.TryGetProbability;
                state.TryGetNosologies = temp.TryGetNosologies;
            }

            if (Enum.TryParse(dialogType, out DialogType dialog)&&
                Enum.TryParse(lang, out Lang language))
            {
                state.DialogType = dialog;
                state.lang = language;
            }
            
            interviewer.SetAnswer(state);
            if (answers != null)
                foreach (Answer answer in answers)
                {
                    interviewer.SetAnswer(state, answer);
                    if (temp!=null)
                        temp.TrySetAnswersFromIsolationSensors(state.DataToIsolationSensor);
                    interviewer.CheckAnsversIntegrity(state);
                    interviewer.GetResponse(state);
                }

            var result = (interviewer.GetResponse(state), state.CurrentQuestionTriggerID.ToString());

            return result;
        }
        
        
        /// <summary>
        /// Метод, генерирующий все страницы
        /// </summary>
        /// <returns></returns>
        public List<InterfaceUnit> GetAllPages(string dialogType = "doctor", string lang = "ru")
        {
            return interviewer.GetAllFields(dialogType , lang);
        }
        public override int GetHashCode()
        {
            return CheckerId.GetHashCode();
        }
    }
}
