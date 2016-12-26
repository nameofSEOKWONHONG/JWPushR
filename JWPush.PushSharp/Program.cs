using JWPush.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JWPush
{
    static class Program
    {
        private const int MIN_THREAD_COUNT = 50;
        private const int MAX_THREAD_COUNT = 3;
        private static List<PushWork> _threadList = new List<PushWork>();
        private static object _lock = new object();

        private static void Main(string[] args)
        {
            int minWorker, minIOC, maxWorker, maxIOC;
            // Get the max, min thread count;
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            ThreadPool.GetMaxThreads(out maxWorker, out maxIOC);

            Console.WriteLine($"Thread work min : {minWorker}, min ioc : {minIOC}");
            Console.WriteLine($"Thread work max : {maxWorker}, max ioc : {maxIOC}");

            Console.WriteLine("Enter Continue.");
            Console.ReadLine();

            ThreadPool.SetMinThreads(minWorker, maxWorker / 2);

            //get work list from db.
            bool isEnded = false;
            while(!isEnded)
            {
                Console.WriteLine("1. add work, 2.work stop, 3.app end.");
                string number = Console.ReadLine();

                switch (number)
                {
                    case "1":
                        if(MAX_THREAD_COUNT < _threadList.Count)
                        {
                            Console.WriteLine("Not create push work theread.");
                            Console.WriteLine("Push work thread is full.");
                            break;
                        }

                        Console.WriteLine("Input work name : ");
                        string workName = Console.ReadLine();

                        if (_threadList.Count <= 0)
                        {
                            PushWork work = new PushWork
                            {
                                Id = 1,
                                WorkName = workName,
                                ThreadId = -1,
                                TokenSource = null,
                                WorkTask = null
                            };
                            work = PushStart(work);
                            lock (_lock)
                            {
                                _threadList.Add(work);
                            }
                        }
                        else
                        {
                            var exist = _threadList.Where(item => item.WorkName == workName)
                                .FirstOrDefault();

                            if(exist != null)
                            {
                                Console.WriteLine("Registered work.");
                            }
                            else
                            {
                                var workId = _threadList.Max(item => item.Id) + 1;
                                PushWork work = new PushWork
                                {
                                    Id = workId + 1,
                                    WorkName = workName,
                                    ThreadId = -1,
                                    TokenSource = null,
                                    WorkTask = null
                                };
                                work = PushStart(work);
                                lock (_lock)
                                {
                                    _threadList.Add(work);
                                }
                            }
                        }
                        break;
                    case "2":
                        Console.WriteLine("Enter thread Id : ");
                        int n = int.Parse(Console.ReadLine());
                        PushStop(n);
                        break;
                    case "3":                        
                    default:
                        isEnded = true;
                        break;
                }
            }

            Console.WriteLine("App terminate.");
            Console.ReadLine();
        }

        public static PushWork PushStart(PushWork w)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;
            Task t = new Task(() =>
            {
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    //get push data from database;
                    //or push service call with create intance.
                    Console.WriteLine($"Working Job : {w}, Thread Id : {Task.CurrentId}");
                    Thread.Sleep(5000);

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine($"Work End : {w}, Thread Id : {Task.CurrentId}");
                        break;
                    }
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }, tokenSource.Token);

            w.ThreadId = t.Id;
            w.WorkTask = t;
            w.TokenSource = tokenSource;

            t.Start();

            return w;
        }

        public static void PushStop(int threadId)
        {
            if(_threadList != null && _threadList.Count > 0)
            {
                lock (_lock)
                {
                    for (int i = _threadList.Count - 1; i >= 0; i--)
                    {
                        if (_threadList[i].ThreadId == threadId)
                        {
                            Console.WriteLine($"Find. Thread Id : {_threadList[i].ThreadId}, Work Name : {_threadList[i].WorkName}");
                            _threadList[i].TokenSource.Cancel();
                            _threadList.Remove(_threadList[i]);
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Work list is empty.");
            }
        }
    }
}
