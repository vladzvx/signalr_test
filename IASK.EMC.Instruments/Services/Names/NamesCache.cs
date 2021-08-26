using IASK.Common.Services;
using IASK.InterviewerEngine;
using System.Collections.Generic;
using System.Linq;
using UMKBRequests.Models.API.Satellite;
using Name = IASK.InterviewerEngine.Name;

namespace IASK.EMC.Instruments
{
    public class NamesCache
    {
        private readonly LifetimeLimitedCache<ulong> cache;
        private readonly string key;
        public NamesCache(LifetimeLimitedCache<ulong> cache, IKeyProvider kostylKeyProvider)
        {
            this.cache = cache;
            key = kostylKeyProvider.GetNamesKey;
        }
        public bool TryGetNames(List<ulong> keys, out List<string> data, NameType nameType = NameType.Name, Lang lang=Lang.ru)
        {
            data = new List<string>();
            List<ulong> keysForRequest = new List<ulong>();
            for (int i = 0; i < keys.Count; i++)
            {
                if (cache.TryGetData<Name>(keys[i], out Name name)&& 
                    name.TryGetName(nameType, lang, out string value))
                {
                    data.Add(value);
                }
                else
                {
                    keysForRequest.Add(keys[i]);
                }
            }
            if (keysForRequest.Count == 0) return true;
            LoadNames(key, keysForRequest);
            foreach (ulong key in keysForRequest)
            {
                if (cache.TryGetData<Name>(key, out Name name) &&
                    name.TryGetName(nameType, lang, out string value))
                {
                    data.Add(value);
                }
            }
            return keys.Count == data.Count;
        }

        private void LoadNames(string apiKey,List<ulong> idsList, NameType nameType = NameType.Name, Lang lang = Lang.ru)
        {
            GetNamesResponse names = CacheForming.GetNames(apiKey, idsList);
            for (int i = 0; i < names.names.Count; ++i)
            {
                if (names.names[i].Count > 0)
                {
                    Name.Builder builder = new Name.Builder();
                    foreach (var item in names.names[i])
                    {
                        builder.TryAddName(item);
                    }
                    Name name = new Name(builder);
                    cache.AddOrUpdateData(idsList.ElementAt(i), name);
                }
            }
        }
    }
}
