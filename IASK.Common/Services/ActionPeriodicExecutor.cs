using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace IASK.Common.Services
{
    public abstract class ActionPeriodicExecutor :IHostedService
    {
        private readonly Timer timer;
        private readonly object locker = new object();

        public ActionPeriodicExecutor(double RepeatingInterval)
        {
            timer = new Timer();
            timer.Interval = RepeatingInterval;
            timer.Elapsed += TimerAction;
            timer.AutoReset = true;
        }

        public virtual void action()
        {

        }

        private void TimerAction(object sender, ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(locker))
            {
                try
                {
                    action();
                }
                finally
                {
                    Monitor.Exit(locker);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Stop();
        }
    }
}
