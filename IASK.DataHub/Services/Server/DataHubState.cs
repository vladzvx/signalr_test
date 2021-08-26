using IASK.Common;
using IASK.DataHub.Interfaces;
using IASK.DataHub.Models;
using IASK.DataStorage.Interfaces;
using IASK.InterviewerEngine;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UMKBRequests;

namespace IASK.DataHub.Services
{
    public class DataHubState<T> : IDataHubState<T> where T : BaseMessage
    {
        public readonly ConcurrentDictionary<string, long> AuthUsers = new ConcurrentDictionary<string, long>();
        public readonly IGroupsManager<T> groupsManager;
        public readonly ICommonWriter<T> commonWriter;
        private readonly IConnectionsFactory connectionsFactory;
        public DataHubState(IDbCreator<T> dbCreator, IConnectionsFactory connectionsFactory, IGroupsManager<T> groupsManager, ICommonWriter<T> commonWriter)
        {
            this.connectionsFactory = connectionsFactory;
            this.groupsManager = groupsManager;
            this.commonWriter = commonWriter;
            using (var conn = connectionsFactory.GetConnectionAsync(CancellationToken.None).Result)
            {
                dbCreator.CreateDB(conn.Connection).Wait();
            }
        }

        public async ValueTask<GroupInfo[]> GetMyGroups(string pwd)
        {
            var groups = await groupsManager.GetUserGroups(await GetUserId(pwd));
            foreach (GroupInfo groupInfo in groups)
            {
                await WriteGroup(groupInfo);
            }
            return groups;
        }

        public async ValueTask<long> GetUserId(string pwd)
        {
            return AuthUsers[pwd];
        }

        public void LogHistory(T data)
        {
            commonWriter.PutData(data);
        }

        public async ValueTask<bool> TryCreateGroup(GroupInfo groupName)
        {
            await WriteGroup(groupName);
            return true;
        }

        public async ValueTask<bool> TryEnterGroup(GroupInfo groupName, string pwd)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<bool> TryLogIn(string pwd, string connectionId)
        {
            var temp = await UMKBWorker.TryGetUserId(pwd);
            if (temp.Success)
            {
                long userId = (long)IdParser.GetNewBigId(Constants.UsersLib, (uint)temp.UserId);
                await WriteUser(userId);
                AuthUsers.TryAdd(connectionId, userId);
            }
            return temp.Success;
        }

        public async ValueTask<List<T>> GetHistory(T LaseRecievedMessage)
        {

            List<T> result = new List<T>();
            using (var connWrapper = await connectionsFactory.GetConnectionAsync(CancellationToken.None))
            {
                DbCommand command = connWrapper.Connection.CreateCommand();
                command.CommandText = "read_history_" + IDbCreator<T>.Name;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new NpgsqlParameter("_group_id", NpgsqlTypes.NpgsqlDbType.Bigint));
                command.Parameters.Add(new NpgsqlParameter("_message_id", NpgsqlTypes.NpgsqlDbType.Bigint));
                command.Parameters["_group_id"].Value = LaseRecievedMessage.GroupId;
                command.Parameters["_message_id"].Value = LaseRecievedMessage.Id;
                using (DbDataReader reader = await command.ExecuteReaderAsync(CancellationToken.None))
                {
                    while (await reader.ReadAsync())
                    {
                        if (!await reader.IsDBNullAsync(0))
                        {
                            string temp = reader.GetString(0);
                            result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(temp));
                        }
                    }
                }
            }
            return result;

            
        }

        public async ValueTask<bool> CheckAutorisation(string connectionId)
        {
            return AuthUsers.ContainsKey(connectionId);
        }

        private async Task WriteUser(long userId)
        {
            using (var connWrapper = await connectionsFactory.GetConnectionAsync(CancellationToken.None))
            {
                DbCommand command = connWrapper.Connection.CreateCommand();
                command.CommandText = string.Format("insert into users_{1} (id) values ('{0}')  on conflict on constraint users_{1}_pkey do nothing;", userId, IDbCreator<T>.Name);
                command.CommandType = System.Data.CommandType.Text;
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task WriteGroup(GroupInfo groupInfo)
        {
            using (var connWrapper = await connectionsFactory.GetConnectionAsync(CancellationToken.None))
            {
                DbCommand command = connWrapper.Connection.CreateCommand();
                command.CommandText = string.Format("insert into groups_{2} (name, id) values ('{0}',{1})  on conflict on constraint groups_{2}_pkey do nothing;", groupInfo.Name, groupInfo.Id, IDbCreator<T>.Name);
                command.CommandType = System.Data.CommandType.Text;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
