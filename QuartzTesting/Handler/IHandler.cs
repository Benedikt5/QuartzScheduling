using QuartzTesting.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting.Handler
{
    public interface IHandler
    {
        Task Handle(string id, IDataSource dataSource);
    }
}
