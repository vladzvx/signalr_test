using System.Collections.Generic;

namespace IASK.InterviewerEngine.Interfaces
{
    public interface IProbabilityCalculator
    {
        IProbabilityCalculator Copy();
        bool TryGetNosologies(out Dictionary<ulong, string> result);
        bool TryGetProbability(InterviewerState.ProbabilityType probabilityType, ulong NosologyId, out double result);
        void TrySetAnswersFromIsolationSensors(Dictionary<ulong, IsolationSensor> DataToIsolationSensor);
    }
}
