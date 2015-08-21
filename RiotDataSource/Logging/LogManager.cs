using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.Logging
{
    public class LogManager
    {
        static List<ILogProcessor> _logProcessors = new List<ILogProcessor>();

        static public void AddLogProcessor(ILogProcessor processor)
        {
            _logProcessors.Add(processor);
        }

        static public void LogMessage(string logMessage)
        {
            foreach (ILogProcessor processor in _logProcessors)
            {
                processor.ProcessLog(logMessage);
            }
        }
    }
}
