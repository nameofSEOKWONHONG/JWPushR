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
            PushServiceHelper psHelper = new PushServiceHelper();

            bool isEnded = false;
            while(!isEnded)
            {
                Console.WriteLine("1. add work, 2.work stop, 3.get works, 4.app end.");
                string number = Console.ReadLine();

                switch (number)
                {
                    case "1":
                        var manageName = Console.ReadLine();
                        var list = psHelper.GetWorkList();

                        if(list.Count <= 0)
                        {
                            psHelper.AddWork(new PushWork
                            {
                                Id = 1,
                                WorkName = manageName
                            });
                        }
                        else
                        {
                            psHelper.AddWork(new PushWork
                            {
                                Id = list.Max(item => item.Id) + 1,
                                WorkName = manageName
                            });
                        }

                        psHelper.Start();
                        break;
                    case "2":
                        Console.WriteLine("Enter thread Id : ");
                        int threadId = int.Parse(Console.ReadLine());
                        var work = psHelper.Stop(threadId);
                        Console.WriteLine($"Id : {work.Id}, Run State : {work.IsRun}, WorkName : {work.WorkName}");
                        break;
                    case "3":
                        var worklist = psHelper.GetWorkList();

                        foreach(var item in worklist)
                        {
                            Console.WriteLine($"Id : {item.Id}, ThreadId : {item.ThreadId}, WorkName : {item.WorkName}, Run State : {item.IsRun}");
                        }

                        break;
                    case "4":                        
                    default:
                        isEnded = true;
                        break;
                }                
            }

            psHelper.Dispose();

            Console.WriteLine("App terminate.");
            Console.ReadLine();
        }
    }
}
