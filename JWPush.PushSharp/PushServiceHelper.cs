#define __RUN_STATE_INFO__
using JWPush.Infrastructure;
using JWPush.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JWPush
{
    public class PushServiceHelper : Disposable
    {
        private const int MIN_THREAD_COUNT = 50;
        private const int MAX_THREAD_COUNT = 3;
        private static List<PushWork> _threadList = new List<PushWork>();
        private static object _lock = new object();

        public List<PushWork> GetWorkList()
        {
            return _threadList;
        }

        public PushServiceHelper AddWork(PushWork work)
        {
            if (MAX_THREAD_COUNT < _threadList.Count)
            {
                Console.WriteLine("Not create push work theread.");
                Console.WriteLine("Push work thread is full.");
                return this;
            }

            lock (_lock)
            {
                _threadList.Add(work);
            }

            return this;
        }

        public void Start()
        {
            foreach (var item in _threadList)
            {
                if (!item.IsRun)
                {
                    var tokenSource = new CancellationTokenSource();
                    CancellationToken ct = tokenSource.Token;
                    Task t = new Task(() =>
                    {
                        ct.ThrowIfCancellationRequested();

                        bool moreToDo = true;
                        while (moreToDo)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                Console.WriteLine($"Work End : {item.WorkName}, Thread Id : {Task.CurrentId}");
                                break;
                            }
#if __RUN_STATE_INFO__
                            //get push data from database;
                            //or push service call with create intance.
                            Console.WriteLine($"Working Job : {item.WorkName}, Thread Id : {Task.CurrentId}");
#endif
                            Thread.Sleep(5000);
                        }
                        Thread.Sleep(1000);
                    }, tokenSource.Token);

                    item.ThreadId = t.Id;
                    item.WorkTask = t;
                    item.TokenSource = tokenSource;
                    item.IsRun = true;

                    t.Start();
                }
            }
        }

        public PushWork Stop(int threadId)
        {            
            if (_threadList != null && _threadList.Count > 0)
            {
                lock (_lock)
                {
                    for (int i = _threadList.Count - 1; i >= 0; i--)
                    {
                        if (_threadList[i].ThreadId == threadId)
                        {
                            Console.WriteLine($"Find. Thread Id : {_threadList[i].ThreadId}, Work Name : {_threadList[i].WorkName}");
                            _threadList[i].TokenSource.Cancel();
                            _threadList[i].IsRun = false;

                            return _threadList[i];                            
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Work list is empty.");
            }

            return null;
        }

        protected override void DisposeCore()
        {
            if(_threadList != null)
            {
                for (int i = _threadList.Count - 1; i >= 0; i--)
                {
                    this.Stop(_threadList[i].Id);
                    Task.WaitAll(_threadList[i].WorkTask);
                }
            }

            _threadList.Clear();


            base.DisposeCore();
        }
    }
}
