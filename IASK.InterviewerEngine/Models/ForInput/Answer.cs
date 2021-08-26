namespace IASK.InterviewerEngine.Models.Input
{
    /// <summary>
    /// Ответ на один вопрос, заданный чекером. 
    /// Используется в качестве структуры для получения ответов в классах <see cref="Interviewer"/> и <see cref="InterviewerWrapper"/>
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Id пациента
        /// </summary>
        public ulong PatientId { get; set; }
        /// <summary>
        /// Id врача
        /// </summary>
        public ulong DoctorId { get; set; }
        /// <summary>
        /// Id триггера, к которому относится ответ.
        /// </summary>
        public string TriggerId { get; set; }
        /// <summary>
        /// Массив ответов на вопросы, заданные в триггере TriggerId
        /// </summary>
        public InputStruct[] CheckerResponse { get; set; }
    }
}
