using Autofac;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using QuartzTesting.DataSource;
using QuartzTesting.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting.Job
{
    public class JobFactory: SimpleJobFactory
    {
        private ILogger _log;
        private IContainer _container;

        public JobFactory(IContainer container)
        {
            _log = container.Resolve<ILogger>();
            _container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var strategy = bundle.JobDetail.JobDataMap.GetString("strategy");
            var provider = bundle.JobDetail.JobDataMap.GetString("connector");
            var rnd = new Random();

            using (var scope = _container.BeginLifetimeScope(x => x.Register(l => new ConsoleLogger(bundle.JobDetail.Key.Name)).As<ILogger>()))
            {
                try
                {
                    var ds = scope.ResolveNamed<IDataSource>(provider);
                    var handler = scope.ResolveNamed<IHandler>(strategy);
                    var job = new SyncJob(handler, ds);
                    return job;
                }
                catch (Exception e)
                {
                    throw new Exception($"Problem with resolving job {bundle.JobDetail.Key}", e);
                }
            }
        }
    }
}
