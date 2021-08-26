using IASK.EMC.Instruments.Services.Converters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.Cases.EMCReader.Services
{
    public class Starter : IHostedService
    {
        private readonly GroupsRepo groupsRepo;
        public Starter(GroupsRepo groupsRepo)
        {
            this.groupsRepo = groupsRepo;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //await groupsRepo.UpdateRepo(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
