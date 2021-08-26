using System.Collections.Immutable;

namespace IASK.InterviewerEngine
{
    internal partial class ComplexDescription
    {
        internal ImmutableDictionary<DialogType, ImmutableDictionary<Lang, Description>> MainDic;
        public bool TryGetDescription(out Description description, 
            DialogType dialog = DialogType.doctor, 
            Lang lang = Lang.ru)
        {
            description = null;
            if (MainDic.ContainsKey(dialog))
            {
                if (MainDic[dialog].ContainsKey(lang))
                {
                    description = MainDic[dialog][lang];
                    return true;
                }
            }
            return false;
        }

        public ComplexDescription(Builder builder)
        {
            var _builder = ImmutableDictionary.CreateBuilder<DialogType, ImmutableDictionary<Lang, Description>>();
            foreach (DialogType key in builder.MainDic.Keys)
            {
                _builder.Add(key, ImmutableDictionary.CreateRange(builder.MainDic[key]));
            }
            MainDic = _builder.ToImmutable();
        }
    }
}
