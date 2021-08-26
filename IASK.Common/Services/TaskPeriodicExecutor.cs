using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace IASK.Common.Services
{
    public abstract class TaskPeriodicExecutor
    {
        private readonly Timer timer;
        private Task workingTask;
        private readonly object locker = new object();
        private CancellationToken cancellationToken;
        internal Action<object?> ExecutingAction { get; private set; }

        public TaskPeriodicExecutor(double RepeatingInterval, Action<object?> action = null)
        {
            this.ExecutingAction = action;
            timer = new Timer();
            timer.Interval = RepeatingInterval;
            timer.Elapsed += TimerAction;
            timer.AutoReset = true;
        }

        public void SetAction(Action<object?> action)
        {
            if (Monitor.TryEnter(locker))
            {
                ExecutingAction = action;
                Monitor.Exit(locker);
            }
        }
        public void Start(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            timer.Start();
        }
        private void TimerAction(object sender, ElapsedEventArgs e)
        {
            if (Monitor.TryEnter(locker))
            {
                if (ExecutingAction != null && (workingTask == null || workingTask.IsCompleted))
                    workingTask = Task.Factory.StartNew(ExecutingAction, cancellationToken, TaskCreationOptions.LongRunning);
                Monitor.Exit(locker);
            }
        }
    }
}
