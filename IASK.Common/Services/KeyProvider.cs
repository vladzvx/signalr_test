using IASK.InterviewerEngine;


namespace IASK.Common.Services
{
    public class KeyProvider : IKeyProvider
    {
        public string SemanticKey => Secrets.Semantic;

        public string SphinxSearchKey => Secrets.SphinxSearch;

        public string DescriptionsListKey => Secrets.Descriptions;

        public string VarcharListKey => Secrets.VarcharList;

        public string BoolSattliteKey => Secrets.BoolSattelite;

        public string GetNamesKey => Secrets.GetNames;


    }
}
