using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using IASK.Common;
using IASK.Common.Models;
using System.Collections.Concurrent;
using IASK.DataHub.Interfaces;
using IASK.DataHub.Models;
using IASK.DataStorage.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Data.Common;
using Npgsql;
using UMKBRequests;
using System.Text.RegularExpressions;

namespace IASK.DataHub.Services
{
    public class DataHubService<T> : Hub where T: BaseMessage
    {
        private readonly DataHubState<T> dataHubState;

        public DataHubService(DataHubState<T> dataHubState)
        {
            this.dataHubState = dataHubState;
        }

        #region server methods
        public async Task LogIn(BaseModel authRequest)
        {
            if (await dataHubState.TryLogIn(authRequest.Permit.authkey,Context.ConnectionId))
            {
                var groups = await dataHubState.GetMyGroups(Context.ConnectionId);
                for (int i = 0; i < groups.Length; i++)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groups[i].Id.ToString());
                }
            }
            else
            {
                Context.Abort();
            }
        }
        public async Task SendToGroup(T data)
        {
            if (await dataHubState.CheckAutorisation(Context.ConnectionId))
            {
                if (data.MessageType != Enums.MessageType.Update && dataHubState.AuthUsers.TryGetValue(Context.ConnectionId, out long userId))
                {
                    data.UserId = userId;
                    data.Id = await dataHubState.groupsManager.GetLastMessageNumber(data.GroupId);
                }
                await Clients.OthersInGroup(data.GroupId.ToString()).SendAsync(data.MessageType.ToString(), data);
                dataHubState.LogHistory(data);
            }
            else Context.Abort();
        }

        public async Task GetHistory(T LaseRecievedMessage)
        {
            if (await dataHubState.CheckAutorisation(Context.ConnectionId))
            {
                var temp = await dataHubState.GetHistory(LaseRecievedMessage);
                await Clients.Caller.SendAsync("History", temp);
            }
            else Context.Abort();
        }
        public async Task RecieveAcknowledgment(T data)
        {
            if (await dataHubState.CheckAutorisation(Context.ConnectionId))
            {
                data.SetAcknowledged();
                await Clients.OthersInGroup(data.GroupId.ToString()).SendAsync(data.MessageType.ToString(), data);
                dataHubState.commonWriter.PutData(data);
            }
            else Context.Abort();
        }
        public async Task GetMyGroups()
        {
            if (await dataHubState.CheckAutorisation(Context.ConnectionId))
            {
                var temp = await dataHubState.GetMyGroups(Context.ConnectionId);
                await Clients.Caller.SendAsync("Groups", temp);
            }
            else Context.Abort();
        }
        #endregion

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            dataHubState.AuthUsers.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
