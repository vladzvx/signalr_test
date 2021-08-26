using IASK.Common.Services;
using IASK.InterviewerEngine;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UMKBRequests;
using UMKBRequests.Models.API.Semantic;

namespace IASK.EMC.Instruments.Services.Converters
{
    public class GroupsRepo
    {
        private readonly IKeyProvider keyProvider;
        private readonly HttpClientWrapper httpClientWrapper;
        internal ConcurrentDictionary<ulong, IEnumerable<ulong>> GroupToItems { get; private set; }
        public GroupsRepo(IKeyProvider keyProvider, HttpClientWrapper httpClientWrapper)
        {
            this.keyProvider = keyProvider;
            this.httpClientWrapper = httpClientWrapper;
            GroupToItems = new ConcurrentDictionary<ulong, IEnumerable<ulong>>();
            UpdateRepo(CancellationToken.None).Wait();
        }

        public async Task<IEnumerable<ulong>> GetGroupMembers(ulong id, CancellationToken token)
        {
            if (GroupToItems.TryGetValue(id, out var result))
            {
                return result;
            }
            else
            {
                IEnumerable<ulong> res = await GetSingleGroup(id, token);
                GroupToItems.TryAdd(id, res);
                return res;
            }
        }

        private async Task<IEnumerable<ulong>> GetSingleGroup(ulong id, CancellationToken cancellationToken)
        {
            RequestSemantic request2 = new RequestSemantic()
            {
                key = keyProvider.SemanticKey,
                level = Constants.LevelToParent,
                route = 0,
                lib = Consts.EMCHeadersLibs,
                libid = new string[1] { id.ToString() },
                deep = 10,
                valmore = true
            };

            ResultSemanticNewBigId branches = await httpClientWrapper.GetResponse<ResultSemanticNewBigId>(request2, cancellationToken);
            if (branches.alerts.code == 200)
            {
                List<ulong> data = branches.graph.Select(item => item.idb).Where(item => Consts.EMCHeadersLibs.Contains(IdParser.ParseNewBigId(item).lib)).ToList() ;
                data.Add(id);
                return data;
            }
            else throw new Exception("Exception while reading Group info: "+Newtonsoft.Json.JsonConvert.SerializeObject(branches.alerts));
        }
        public async Task UpdateRepo(CancellationToken cancellationToken)
        {
            GroupToItems.Clear();

            RequestSemantic request = new RequestSemantic()
            {
                key=keyProvider.SemanticKey,
                level = Constants.LevelTriggerSensor,
                route = Constants.RouteTriggerSensor,
                lib = Constants.LibChecker,
                libid = Consts.GroupsRepoStarts,
                deep = 1,
                valmore = true
            };

            ResultSemanticNewBigId res = await httpClientWrapper.GetResponse<ResultSemanticNewBigId>(request, cancellationToken);

            if (res.alerts.code == 200)
            {
                
                IEnumerable<ulong> menuItemsIds = res.graph.Select(item => item.idb);
                menuItemsIds = menuItemsIds.Where(item => Consts.EMCHeadersLibs.Contains(IdParser.ParseNewBigId(item).lib));
                foreach (ulong id in menuItemsIds)
                {
                    GroupToItems.TryAdd(id, await GetSingleGroup(id, cancellationToken));
                }
            }
            else throw new Exception("GroupsRepo creating failde!");
        }
    }
}
