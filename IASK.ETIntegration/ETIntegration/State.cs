using ETALib.Requests;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace IASK.ETIntegration.Services
{
    public class State
    {
        public State()
        {
            timer.Elapsed += Clear;
            timer.AutoReset = true;
            timer.Start();
        }

        private object locker = new object();
        public Timer timer = new Timer(5000);
        public string QuestionRedirectAdress = Environment.GetEnvironmentVariable("QuestionRedirectAdress");
        public string StartDataRedirectAdress = Environment.GetEnvironmentVariable("StartDataRedirectAdress");
        public ConcurrentDictionary<Guid, QuestionRequest> Questions = new ConcurrentDictionary<Guid, QuestionRequest>();
        public ConcurrentDictionary<Guid, DateTime> QuestionsTimes = new ConcurrentDictionary<Guid, DateTime>();
        public ConcurrentDictionary<Guid, SubmitTableRequest> Results = new ConcurrentDictionary<Guid, SubmitTableRequest>();
        public ConcurrentDictionary<Guid, DateTime> ResultsTimes = new ConcurrentDictionary<Guid, DateTime>();

        public bool TryGetQuestion(Guid guid, out QuestionRequest questionRequest)
        {
            if (Questions.TryRemove(guid, out questionRequest))
            {
                QuestionsTimes.TryRemove(guid, out var temp);
                return true;
            }
            return false;
        }

        public bool TryGetResult(Guid guid, out SubmitTableRequest submitTable)
        {
            if (Results.TryRemove(guid, out submitTable))
            {
                ResultsTimes.TryRemove(guid, out var temp);
                return true;
            }
            return false;
        }
        public bool TryAdd(Guid guid, QuestionRequest request)
        {
            return Questions.TryAdd(guid, request) && QuestionsTimes.TryAdd(guid,DateTime.UtcNow);
        }

        public bool TryAdd(Guid guid, SubmitTableRequest request)
        {
            return Results.TryAdd(guid, request) && ResultsTimes.TryAdd(guid, DateTime.UtcNow);
        }

        private void Clear(object sender, ElapsedEventArgs args)
        {
            if (Monitor.TryEnter(locker))
            {
                try
                {
                    foreach (Guid guid in Questions.Keys.ToArray())
                    {
                        if (QuestionsTimes.TryGetValue(guid, out DateTime dt) && DateTime.UtcNow.Subtract(dt).TotalMinutes > 2)
                        {
                            Questions.TryRemove(guid, out var t1);
                            QuestionsTimes.TryRemove(guid, out var t2);
                        }
                    }
                    foreach (Guid guid in Results.Keys.ToArray())
                    {
                        if (ResultsTimes.TryGetValue(guid, out DateTime dt))
                        {
                            Results.TryRemove(guid, out var t1);
                            ResultsTimes.TryRemove(guid, out var t2);
                        }
                    }
                }
                catch { }
                Monitor.Exit(locker);
            }
        }
    }
}
