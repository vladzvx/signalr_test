using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.DataStorage.Services.Common
{
    public class CommonWriter<T> : TaskPeriodicExecutor, ICommonWriter<T> where T : class
    {
        private readonly ConcurrentQueue<T> DataQueue = new ConcurrentQueue<T>();
        private readonly ConcurrentQueue<MultipleDataWrapper<T>> MultipleDataQueue = new ConcurrentQueue<MultipleDataWrapper<T>>();
        private readonly IWriterCore<T> writerCore;
        private readonly IConnectionsFactory manager;
        private readonly IDataBaseSettings settings;
        public CommonWriter(IWriterCore<T> writerCore, IDataBaseSettings settings, IConnectionsFactory manager) :
            base(settings.StartWritingInterval)
        {
            this.writerCore = writerCore;
            this.manager = manager;
            this.settings = settings;
            this.SetAction(WritingActionWrapper);
            Start(CancellationToken.None);
        }
        public void PutData(T data)
        {
            if (data == null) throw new ArgumentNullException();
            DataQueue.Enqueue(data);
        }
        public void PutData(MultipleDataWrapper<T> data)
        {
            if (data == null || data.data==null) throw new ArgumentNullException();
            MultipleDataQueue.Enqueue(data);
        }
        private void WritingActionWrapper(object CancellationToken)
        {
            if (!(CancellationToken is CancellationToken forceStopToken)) return;
            WritingAction(forceStopToken).Wait();
        }
        private async Task WritingAction(CancellationToken forceStopToken)
        {
            using (IConnectionWrapper connectionWrapper = await manager.GetConnectionAsync(forceStopToken))
            {
                DbCommand command = writerCore.CreateMainCommand(connectionWrapper.Connection);
                while (!forceStopToken.IsCancellationRequested && !(DataQueue.IsEmpty&& MultipleDataQueue.IsEmpty))
                {
                    using (DbTransaction transaction = await connectionWrapper.Connection.BeginTransactionAsync(forceStopToken))
                    {
                        List<MultipleDataWrapper<T>> wrappers = new List<MultipleDataWrapper<T>>();
                        try
                        {
                            for (int i = 0; i < settings.TrasactionSize && !forceStopToken.IsCancellationRequested && !(DataQueue.IsEmpty && MultipleDataQueue.IsEmpty); i++)
                            {
                                if (DataQueue.TryDequeue(out T message))
                                {
                                    await writerCore.ExecuteWriting(command, message, forceStopToken);
                                }
                                if (MultipleDataQueue.TryPeek(out MultipleDataWrapper<T> result)) 
                                {
                                    MultipleDataQueue.TryDequeue(out _);
                                    foreach (T item in result.data)
                                    {
                                        await writerCore.ExecuteWriting(command, item, forceStopToken);
                                    }
                                    wrappers.Add(result);
                                    i += result.data.Length;
                                }
                            }
                            await transaction.CommitAsync(forceStopToken);
                            foreach (MultipleDataWrapper<T> wrapper in wrappers)
                            {
                                wrapper.Complited(WritingStatus.Writed);
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            foreach (MultipleDataWrapper<T> wrapper in wrappers)
                            {
                                wrapper.Complited(WritingStatus.Failed);
                            }
                        }
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}
