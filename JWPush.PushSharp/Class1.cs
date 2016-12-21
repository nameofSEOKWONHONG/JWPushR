using JWPush.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JWPush
{
    static class Program
    {
        private const int MIN_THREAD_COUNT = 50;
        private const int MAX_THREAD_COUNT = 50;
        private static List<PushWork> _threadList = new List<PushWork>();
        private static object _lock = new object();

        private static void Main(string[] args)
        {
            int minWorker, minIOC, maxWorker, maxIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            ThreadPool.GetMaxThreads(out maxWorker, out maxIOC);

            Console.WriteLine($"Thread work min : {minWorker}, min ioc : {minIOC}");
            Console.WriteLine($"Thread work max : {maxWorker}, max ioc : {maxIOC}");

            Console.ReadLine();

            ThreadPool.SetMinThreads(MIN_THREAD_COUNT, MAX_THREAD_COUNT);

            //get work list from db.
            List<string> work = new List<string>();
            work.Add("test1");
            work.Add("test2");

            List<PushWork> taskList = new List<PushWork>();
            
            foreach (var w in work)
            {
                var tokenSource = new CancellationTokenSource();
                CancellationToken ct = tokenSource.Token;
                Task t = new Task(() =>
                {
                    ct.ThrowIfCancellationRequested();

                    bool moreToDo = true;
                    while(moreToDo)
                    {                        
                        Console.WriteLine($"Working Job : {w}, Thread Id : {Task.CurrentId}");
                        if(ct.IsCancellationRequested)
                        {                            
                            Console.WriteLine($"Work End : {w}, Thread Id : {Task.CurrentId}");
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(1000);
                }, tokenSource.Token);

                taskList.Add(new PushWork
                {
                    ThreadId = t.Id,
                    TokenSource = tokenSource,
                    WorkName = w,
                    WorkTask = t
                });
                t.Start();
            }

            Input:
            var input = Console.ReadLine();

            bool isTerminate = false;
            for(int i=taskList.Count - 1; i>=0; i--)
            {
                if(taskList[i].ThreadId == int.Parse(input))
                {
                    Console.WriteLine($"Find. Thread Id : {taskList[i].ThreadId}, Work Name : {taskList[i].WorkName}");
                    taskList[i].TokenSource.Cancel();
                    taskList.Remove(taskList[i]);                    
                }

                if (taskList.Count <= 0) isTerminate = true;
            }

            if (!isTerminate) goto Input;
            

            Console.WriteLine("Work all end.");
            Console.ReadLine();
        }


        //public static void WorkPush(int threadId)
        //{
        //    lock(_lock)
        //    {
        //        if(_threadList.Contains(threadId)) {
        //            _threadList.Add(threadId);
        //        }

        //        while(true) {

        //        }
        //    }
        //}
    }

    public class PushWork
    {
        public int ThreadId { get; set; }
        public Task WorkTask { get; set; }        
        public string WorkName { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
    }
}
