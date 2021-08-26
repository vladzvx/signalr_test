using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Utils
{
    public class MultipleDataWrapper<T> where T : class
    {
        public WritingStatus status;
        public readonly T[] data;
        public readonly Task<WritingStatus> waiting;
        private readonly CancellationTokenSource cts;

        public MultipleDataWrapper(IEnumerable<T> data)
        {
            cts = new CancellationTokenSource();
            this.data = data.ToArray();
            Task temp = Task.Delay(-1, cts.Token);
            waiting = temp.ContinueWith<WritingStatus>((previousTaskResult) =>
            {
                return this.status;
            });
        }

        public void Complited(WritingStatus status)
        {
            this.status = status;
            cts.Cancel();
        }
    }
}
