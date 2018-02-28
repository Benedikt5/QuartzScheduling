using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting.Handler
{
    public class EventDrivenHandler : IHandler
    {
        ILogger _log;
        public EventDrivenHandler(ILogger log)
        {
            _log = log;
        }
        public async Task Handle(string id)
        {
            await _log.Log($"EventDriven handle handles source {id}");
        }
    }
}
