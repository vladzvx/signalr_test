using IASK.DataHub.Interfaces;
using IASK.DataHub.Models;
using IASK.InterviewerEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMKBRequests;
using UMKBRequests.Models.API.Semantic;

namespace IASK.DataHub.Services
{
    public class InputFormGroupsManager : IGroupsManager<FormMessage>
    {
        private readonly IKeyProvider keyProvider;
        private readonly ConcurrentDictionary<long, long> LastMessages = new ConcurrentDictionary<long, long>();
        public InputFormGroupsManager(IKeyProvider keyProvider)
        {
            this.keyProvider = keyProvider;
        }
        public async ValueTask<long> GetLastMessageNumber(long groupId)
        {
            return LastMessages.AddOrUpdate(groupId, 1, (key, oldValue) => oldValue + 1);
        }
        public async ValueTask<GroupInfo[]> GetUserGroups(long userId)
        {
            if (userId == 0)
            {
                GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
                RequestSemantic settingGraph = new RequestSemantic()
                {
                    key = keyProvider.SemanticKey,
                    level = new ushort[1] { 1050 },
                    route = 0,
                    lib = new ushort[1] { 123 },
                    libid = new string[1] { IdParser.GetNewBigId(123, 229).ToString() },
                    deep = 10,
                };
                var result = getSemantic.Get(settingGraph);
                var temp = result.graph.Where(item => item.levelb == 955).Select(item => new GroupInfo() { Id = (long)item.idb, Name = result.names[item.idb] });

                return temp.ToArray();
            }
            else
            {
                GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
                RequestSemantic settingGraph = new RequestSemantic()
                {
                    key = keyProvider.SemanticKey,
                    level = new ushort[1] { 9112 },
                    route = 0,
                    lib = new ushort[1] { 120 },
                    libid = new string[1] { userId.ToString() },
                    deep = 1,
                };
                ResultSemanticNewBigId result = getSemantic.Get(settingGraph);

                var temp = result.graph.Where(item => item.levelb == 952).Select(item => item.idb).ToArray();
                settingGraph = new RequestSemantic()
                {
                    key = keyProvider.SemanticKey,
                    level = new ushort[1] { 957 },
                    route = 0,
                    lib = new ushort[1] { 123 },
                    libid = new string[1] { temp[0].ToString() },
                    deep = 1,
                };
                result = getSemantic.Get(settingGraph);
                var temp2 = result.graph.Where(item => item.levelb == 955).Select(item => new GroupInfo() { Id = (long)item.idb, Name = result.names[item.idb] }).ToArray();
                return temp2;
            }
        }
    }
}
