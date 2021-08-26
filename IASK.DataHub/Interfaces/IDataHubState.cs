using IASK.Common.Models;
using IASK.DataHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IASK.DataHub.Interfaces
{
    public interface IDataHubState<T> where T:BaseMessage
    {
        public ValueTask<bool> TryLogIn(string pwd, string connectionId);
        public ValueTask<long> GetUserId(string pwd);
        public void LogHistory(T data);
        public ValueTask<bool> TryCreateGroup(GroupInfo groupInfo);
        public ValueTask<bool> TryEnterGroup(GroupInfo groupInfo, string pwd);
        public ValueTask<GroupInfo[]> GetMyGroups(string pwd);
    }
}
