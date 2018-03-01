using Autofac;
using Quartz;
using Quartz.Impl;
using QuartzTesting.DataSource;
using QuartzTesting.Handler;
using QuartzTesting.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting
{
    class Program
    {
        static IScheduler _scheduler;

        static List<Config> _sources;
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            _sources = new List<Config>
            {
                new Config("KZ_excel","excel", new Dictionary<string, string[]>{{"full", new string[] { "0/5 * * * * ?", "0/7 * * * * ?" } } })//, {"event", new string[] { "0/15 * * * * ?", "0/25 * * * * ?" } } }),
                //new Config("KZ_ ad", "ad", new Dictionary<string, string[]>{{"full", ["0/3 * * * * ?", "0/5 * * * * ?"] }, {"event", ["0/4 * * * * ?", "0/5 * * * * ?"] } })
            };

            RunAsync().GetAwaiter().GetResult();

            Console.ReadKey();

        }


        static async Task RunAsync()
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLogger>().As<ILogger>();
            builder.RegisterType<FullHandler>().Named<IHandler>("full");
            builder.RegisterType<EventDrivenHandler>().Named<IHandler>("event");
            builder.RegisterType<ExcelDataSource>().Named<IDataSource>("excel");
            builder.RegisterType<AdDataSource>().Named<IDataSource>("ad");
            var container = builder.Build();

            var jobCounter = 1;
            foreach (var source in _sources)
            {
                foreach (var strategy in source.Schedule)
                {
                    var worker = JobBuilder.Create<SyncJob>()
                        .WithIdentity($"job{jobCounter}({source.Id})", "worker")
                        .StoreDurably()
                        .Build();

                    worker.JobDataMap.Put("id", source.Id);
                    worker.JobDataMap.Put("strategy", strategy.Key);
                    worker.JobDataMap.Put("connector", source.Provider);


                    
                    var triggerSet = new List<ITrigger>();
                    foreach (var sched in strategy.Value)
                    {
                        var tb = TriggerBuilder.Create()
                            .WithIdentity($"trigger[{source.Id}][{jobCounter}]", "worker")
                            .WithCronSchedule(sched)
                            .Build();
                        triggerSet.Add(tb);
                        jobCounter++;
                    }
                    
                    

                    await _scheduler.ScheduleJob(worker, triggerSet,true);
                    jobCounter++;
                }
            }



            var jobFactory = new JobFactory(container);
            _scheduler.JobFactory = jobFactory;

            await _scheduler.Start();
            Console.ReadKey();
            await _scheduler.Shutdown();
        }
    }
}
