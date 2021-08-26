using System;
using System.Collections.Generic;

namespace IASK.InterviewerEngine
{
    public partial class Name
    {
        public class Builder
        {
            internal Dictionary<NameType, Dictionary<Lang, string>> Names = new Dictionary<NameType, Dictionary<Lang, string>>();

            public bool TryAddName(UMKBRequests.Models.API.Satellite.Name name)
            {
                if (Enum.TryParse(name.field,true, out NameType nameType) && Enum.TryParse(name.lang, true, out Lang lang))
                {
                    return TryAddName(nameType, lang, name.text);
                }
                return false;
            }

            public bool TryAddName(NameType nameType, Lang lang, string name)
            {
                bool result;
                if (Names.ContainsKey(nameType))
                {
                    result = Names[nameType].TryAdd(lang, name);
                }
                else
                {
                    Names.Add(nameType, new Dictionary<Lang, string>() { {lang, name } });
                    result = true;
                }
                return result;
            }
        }
    }
}
