using QuartzTesting.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting.Handler
{
    public class FullHandler : IHandler, IDisposable
    {
        private ILogger _log;

        public FullHandler(ILogger log)
        {
            _log = log;
        }

        public void Dispose()
        {
            _log.Log("Dispose full").GetAwaiter().GetResult();
        }

        public async Task Handle(string id, IDataSource dataSource)
        {
            await _log.Log($"Full {dataSource.Name}, id = {id}");
        }
    }
}
