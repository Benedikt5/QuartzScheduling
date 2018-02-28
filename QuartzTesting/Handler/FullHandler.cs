using QuartzTesting.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting.Handler
{
    public class FullHandler : IHandler
    {
        private ILogger _log;

        public FullHandler(ILogger log)
        {
            _log = log;
        }
        public async Task Handle(string id, IDataSource dataSource)
        {
            await _log.Log($"Full handle handles dataSource source {dataSource.Name}, id = {id}");
        }
    }
}
