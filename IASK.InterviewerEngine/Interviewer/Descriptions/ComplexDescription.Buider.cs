using System;
using System.Collections.Generic;
using fieldsDescriptions = UMKBNeedStuff.Models.FieldsDescriptions;

namespace IASK.InterviewerEngine
{
    internal partial class ComplexDescription
    {
        internal class Builder
        {
            internal Dictionary<DialogType, Dictionary<Lang, Description>> MainDic = 
                new Dictionary<DialogType, Dictionary<Lang, Description>>();
            public bool TryAddDescriptions(fieldsDescriptions descriptions)
            {
                if (Enum.TryParse<DialogType>(descriptions.field, out DialogType dialogType) &&
                    Enum.TryParse<Lang>(descriptions.lang, out Lang lang))
                {
                    if (MainDic.ContainsKey(dialogType))
                    {
                        MainDic[dialogType].TryAdd(lang, descriptions);
                    }
                    else
                    {
                        Dictionary<Lang, Description> descs = new Dictionary<Lang, Description>();
                        descs.TryAdd(lang, descriptions);
                        MainDic.TryAdd(dialogType, descs);
                    }
                }
                return false;
            }
        }
    }
}
