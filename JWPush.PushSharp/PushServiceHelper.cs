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
        private readonly int MIN_THREAD_COUNT = 50;
        private readonly int MAX_THREAD_COUNT = 3;
        private const int DIV_THREAD_COUNT = 6;

        private static List<PushWork> _threadList = new List<PushWork>();
        private static object _lock = new object();

        public PushServiceHelper()
        {
            int minWorker, minIOC, maxWorker, maxIOC;
            // Get the max, min thread count;
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            ThreadPool.GetMaxThreads(out maxWorker, out maxIOC);

#if DEBUG
            Console.WriteLine($"Thread work min : {minWorker}, min ioc : {minIOC}");
            Console.WriteLine($"Thread work max : {maxWorker}, max ioc : {maxIOC}");

            MIN_THREAD_COUNT = minWorker;
            MAX_THREAD_COUNT = 3;
#else
            MIN_THREAD_COUNT = minWorker;
            MAX_THREAD_COUNT = maxWorker / DIV_THREAD_COUNT;
#endif
        }

        public List<PushWork> GetWorkList()
        {
            return _threadList;
        }

        public PushServiceHelper AddWork(PushWork work)
        {
            if (MAX_THREAD_COUNT < _threadList.Count)
            {
                //Console.WriteLine("Not create push work theread.");
                //Console.WriteLine("Push work thread is full.");
                throw new Exception("Not create push work theread. Push work thread is full.");                
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
#if DEBUG
                                Console.WriteLine($"Work End : {item.WorkName}, Thread Id : {Task.CurrentId}");
#else
                                //write log
#endif
                                break;
                            }

                            //get push data from database;
                            //use PushHelper

                            //or push service call with create intance.
                            //use IJWPush
#if DEBUG
                            Console.WriteLine($"Working Job : {item.WorkName}, Thread Id : {Task.CurrentId}");
#else
                            //write log
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
#if DEBUG
                            Console.WriteLine($"Find. Thread Id : {_threadList[i].ThreadId}, Work Name : {_threadList[i].WorkName}");
#else
                            //write log
#endif
                            _threadList[i].TokenSource.Cancel();
                            _threadList[i].IsRun = false;

                            return _threadList[i];                            
                        }
                    }
                }
            }
            else
            {
#if DEBUG
                Console.WriteLine("Work list is empty.");
#else
                //write log
#endif
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
