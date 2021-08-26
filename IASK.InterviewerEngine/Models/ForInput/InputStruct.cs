namespace IASK.InterviewerEngine.Models.Input
{
    /// <summary>
    /// Одиночный ответ на вопрос, заданный вопросно-ответной системой <see cref="Interviewer"/>
    /// </summary>
    public class InputStruct
    {
        public string EMCId { get; set; }
        /// <summary>
        /// id сенсора, значение вставляется из <see cref="IASK.InterviewerEngine.Models.Output.InterfaceUnit.Id"/>
        /// при ответе на сенсор.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// id сенсора, значение вставляется из <see cref="IASK.InterviewerEngine.Models.Output.InterfaceUnit.Idb"/>
        /// при ответе на сенсор.
        /// </summary>
        public string Idb { get; set; }
        /// <summary>
        /// Ответ пользователя. 
        /// </summary>
        public string Value { get; set; }
    }
}
