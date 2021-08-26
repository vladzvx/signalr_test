namespace IASK.InterviewerEngine.Interfaces
{
    public interface IProbabilityCalculatorFactory
    {
        bool TryGetProbabilityCalculator(ulong id, out IProbabilityCalculator probabilityCalculator);
    }
}
