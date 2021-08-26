namespace IASK.InterviewerEngine
{
    public interface IKeyProvider
    {
        public string SemanticKey { get; }
        public string SphinxSearchKey { get; }
        public string DescriptionsListKey { get;}
        public string VarcharListKey { get; }
        public string BoolSattliteKey { get; }
        public string GetNamesKey { get; }
    }
}
