using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace IASK.Common.Services
{
    public interface IChacheSettings
    {
        public double CheckingInterval { get; set; }

        public TimeSpan Lifetime { get; set; }
    }

    public class ChacheSettings : IChacheSettings
    {
        public double CheckingInterval { get => 5000; set => throw new NotImplementedException(); }
        public TimeSpan Lifetime { get => new TimeSpan(0,1,0); set => throw new NotImplementedException(); }
    }
    public class LifetimeLimitedCache<T> where T : struct
    {
        private readonly Timer timer;
        private readonly ConcurrentDictionary<T, object> MainDict;
        private readonly ConcurrentDictionary<T, DateTime> DateTimeDict;
        private readonly TimeSpan Lifetime;

        public LifetimeLimitedCache(IChacheSettings settings)
        {
            MainDict = new ConcurrentDictionary<T, object>();
            DateTimeDict = new ConcurrentDictionary<T, DateTime>();
            timer = new Timer();
            Lifetime = settings.Lifetime;
            timer.Interval = settings.CheckingInterval;
            timer.AutoReset = true;
            timer.Elapsed += Clearing;
            timer.Start();
        }

        public bool TryGetData<TData>(T id, out TData result) where TData : class
        {
            if (MainDict.TryGetValue(id,out object data) && data is TData)
            {
                DateTimeDict[id] = DateTime.UtcNow;
                result = (TData)data;
                return true;
            }
            result = null;
            return false;
        }

        public bool TryGetData(T id, out object result)
        {
            if (MainDict.TryGetValue(id, out result))
            {
                DateTimeDict[id] = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public bool AddOrUpdateData(T id, object data)
        {
            DateTime dt = DateTime.UtcNow;
            MainDict.AddOrUpdate(id, data, (id, oldValue) => data);
            DateTimeDict.AddOrUpdate(id, dt, (id, oldValue) => dt);
            return true;
        }

        private void Clearing(object sender, ElapsedEventArgs args)
        {
            foreach (T key in DateTimeDict.Keys.ToArray())
            {
                if (DateTimeDict.TryGetValue(key,out DateTime dt))
                {
                    if (DateTime.UtcNow - dt > Lifetime)
                    {
                        DateTimeDict.TryRemove(key, out _);
                        MainDict.TryRemove(key, out _);
                    }
                }
            }
        }
    }
}
