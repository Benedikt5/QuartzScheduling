using Quartz;
using QuartzTesting.DataSource;
using QuartzTesting.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting
{
    public class SyncJob: IJob
    {
        private IHandler _handler;
        private IDataSource _ds;

        public SyncJob(IHandler handler, IDataSource ds)
        {
            _handler = handler;
            _ds = ds;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var id = context.JobDetail.JobDataMap.GetString("id");

            await _handler.Handle(id, _ds);
        }
    }
}
