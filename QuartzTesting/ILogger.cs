using System.Threading.Tasks;

namespace QuartzTesting
{
    public interface ILogger
    {
        Task Log(string msg);
    }
}