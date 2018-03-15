using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzTesting
{
    public class Config
    {
///<summary>
/// samplesss
///</summary>
        public string Id { get; set; }
        public string Provider { get; set; }
        public Dictionary<string,string[]> Schedule { get; set; }
        public Config(string id, string provider, Dictionary<string,string[]> sched)
        {
            Id = id;
            Provider = provider;
            Schedule = sched;
        }
    }
}
