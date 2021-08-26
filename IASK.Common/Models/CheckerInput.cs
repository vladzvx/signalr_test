using IASK.InterviewerEngine.Models.Input;
using System;
using System.Collections.Generic;

namespace IASK.Common.Models
{
    /// <summary>
    /// Модель для входящего запроса к вопросно-ответной системе. 
    /// </summary>
    public class CheckerInput : BaseModel
    {
        /// <summary>
        /// Уникальный идентификатор операции/пользователя
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Профиль пользователя: врач, пацент, ЛПУ и т.д. Принимает один из следующих вариантов: 
        /// </summary>
        public string Dialog { get; set; }

        /// <summary>
        /// Используемый язык. По умолчанию - русский.
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// Либ чекера
        /// </summary>
        public ushort Lib { get; set; }
        /// <summary>
        /// Id чекера
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// Ответы пользователя. null соответсвует запроса стартовой страницы.
        /// </summary>
        public List<Answer> Answers { get; set; }
    }

}
