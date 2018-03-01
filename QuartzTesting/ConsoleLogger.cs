using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting
{
    public class ConsoleLogger : ILogger
    {
        private string _name;

        public ConsoleLogger()
        {
            _name = "DefaultConsoleLogger";
        }
        public ConsoleLogger(string name)
        {
            _name = name;
        }
        public async Task Log(string msg)
        {
            await Console.Out.WriteLineAsync($"{DateTime.Now.TimeOfDay}|{_name}: {msg}");
        }
    }
}
