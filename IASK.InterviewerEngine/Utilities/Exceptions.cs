using System;
using Alert = UMKBRequests.Models.API.Semantic.Alert;

namespace IASK.InterviewerEngine.Exceptions
{
    /// <summary>
    /// Исключение, вызываемое при нарушении последовательности/единственности ответов в вопросно-ответной системе <see cref="Interviewer"/>.
    /// </summary>
    public class UncorrectAnsverException : Exception
    {
        public UncorrectAnsverException(string message):base(message)
        {

        }
    }

    /// <summary>
    /// Исключение, вызываемое при провале попытки запроса графов из UMKB с помощью <see cref="UMKBRequests.GetSemanticNewBigId"/>
    /// </summary>
    public class GetSemanticException : Exception
    {
        internal GetSemanticException(Alert alert, string message = null) :
            base(string.IsNullOrEmpty(message) ?
                string.Format("GetSemanticNewBigId failed! Code: {0}; Message: {1}", alert.code, alert.message) :
                string.Format("GetSemanticNewBigId failed! Code: {0}; Message: {1}; Additional Message: {2}", alert.code, alert.message, message))
        {

        }
    }

    /// <summary>
    /// Исключение, вызываемое при провале попытки получения описаний концептов из БД с помощью <see cref="UMKBRequests.GetDescriptionsDB"/>
    /// </summary>
    public class GetDescriptionsException : Exception
    {
        internal GetDescriptionsException(UMKBNeedStuff.Alert alert, string message = null) :
            base(string.IsNullOrEmpty(message) ? string.Format("GetDescriptionsDB failed! Code: {0}; Message: {1}", alert.code, alert.message) :
                string.Format("GetDescriptionsDB failed! Code: {0}; Message: {1}; Additional Message: {2}", alert.code, alert.message, message))
        {
        }
    }

    /// <summary>
    /// Исключение, вызываемое при провале попытки полученияvarchar концептов <see cref="UMKBRequests.GetVarCharList"/>
    /// </summary>
    public class GetVarcharListException : Exception
    {
        internal GetVarcharListException(UMKBRequests.Models.API.Semantic.Alert alert, string message = null) :
            base(string.IsNullOrEmpty(message) ? string.Format("GetDescriptionsDB failed! Code: {0}; Message: {1}", alert.code, alert.message) :
                string.Format("GetDescriptionsDB failed! Code: {0}; Message: {1}; Additional Message: {2}", alert.code, alert.message, message))
        {
        }
    }

    /// <summary>
    /// Исключение, вызываемое при провале попытки получения названий концептов с помощью  <see cref="UMKBRequests.GetNames"/>
    /// </summary>
    public class GetNamesException : Exception
    {
        internal GetNamesException(UMKBRequests.Alert alert, string message = null) :
            base(string.IsNullOrEmpty(message) ? string.Format("GetGetNames failed! Code: {0}; Message: {1}", alert.code, alert.message) :
                string.Format("GetNames failed! Code: {0}; Message: {1}; Additional Message: {2}", alert.code, alert.message, message))
        {
        }
    }
}
