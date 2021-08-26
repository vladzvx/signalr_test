using IASK.DataHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IASK.DataHub.Interfaces
{
    public interface IGroupsManager<T> where T:BaseMessage
    {
        public ValueTask<GroupInfo[]> GetUserGroups(long userId);

        public ValueTask<long> GetLastMessageNumber(long groupId);
    }
}
