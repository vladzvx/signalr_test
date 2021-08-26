using System.Collections.Immutable;

namespace IASK.InterviewerEngine
{
    public partial class Name
    {
        internal ImmutableDictionary<NameType, ImmutableDictionary<Lang, string>> Names;

        public Name(Builder builder)
        {
            var NamesBuilder = ImmutableDictionary.CreateBuilder<NameType, ImmutableDictionary<Lang, string>>();
            foreach (NameType nameType in builder.Names.Keys)
            {
                NamesBuilder.Add(nameType, ImmutableDictionary.CreateRange(builder.Names[nameType]));
            }
            Names = NamesBuilder.ToImmutable();
        }

        public bool TryGetName(NameType nameType, Lang lang, out string value)
        {
            value = string.Empty;
            return Names.TryGetValue(nameType, out var dict) && dict.TryGetValue(lang, out value);
        }
    }
}
