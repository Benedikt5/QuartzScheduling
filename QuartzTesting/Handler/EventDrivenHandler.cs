using QuartzTesting.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzTesting.Handler
{
    public class EventDrivenHandler : IHandler, IDisposable
    {
        ILogger _log;
        public EventDrivenHandler(ILogger log)
        {
            _log = log;
        }

        public void Dispose()
        {
            Thread.Sleep(1000);
            _log.Log("Dispose event").GetAwaiter().GetResult();
        }

        public async Task Handle(string id, IDataSource dataSource)
        {
            await _log.Log($"Event {dataSource.Name}, id = {id}");
        }
    }
}
