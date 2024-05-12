using Serilog;
using WebApplication3.Interfaces;

namespace WebApplication3.Services
{
    public class LoggerService : ILoggerService
    {
        public void LogMessage(string message)
        {
            Log.Information(message);
        }
    }
}
