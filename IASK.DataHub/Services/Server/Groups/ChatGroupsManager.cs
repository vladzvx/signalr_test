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
    public class DefaultGroupsManager<T> : IGroupsManager<T> where T : BaseMessage
    {
        private readonly ConcurrentDictionary<long, long> LastMessages = new ConcurrentDictionary<long, long>();
        public DefaultGroupsManager()
        {

        }
        public async ValueTask<long> GetLastMessageNumber(long groupId)
        {
            return LastMessages.AddOrUpdate(groupId, 1, (key, oldValue) => oldValue + 1);
        }
        public async ValueTask<GroupInfo[]> GetUserGroups(long userId)
        {
            return new GroupInfo[1] { new GroupInfo() { Id = 0, Name="Common" } };
        }
    }
}
