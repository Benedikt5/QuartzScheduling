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
                new Config("KZ_excel","excel", new Dictionary<string, string>{{"full", "0/20 * * * * ?" }, {"event", "0/21 * * * * ?" } }),
                new Config("KZ_ ad", "ad", new Dictionary<string, string>{{"full", "0/15 * * * * ?" }, {"event", "0/16 * * * * ?" } })
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
            foreach (var source in _sources)
            {
                foreach(var strategy in source.Schedule)
                {
                    var worker = JobBuilder.Create<SyncJob>()
                        .WithIdentity(source.Id, "worker")
                        .StoreDurably()
                        .UsingJobData(new JobDataMap())
                        .Build();
                    var trigger = TriggerBuilder.Create()
                        .WithIdentity("trig1", "worker")
                        .WithCronSchedule(strategy.Value)
                        .Build();

                    await _scheduler.ScheduleJob(worker, trigger);
                }
            }

            builder.RegisterType<AdDataSource>().Named<IDataSource>("ad");
            var container = builder.Build();

            var jobFactory = new JobFactory(container);
            _scheduler.JobFactory = jobFactory;

            
        }
    }
}
