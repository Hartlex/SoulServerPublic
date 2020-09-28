using NetworkCommsDotNet;
using NetworkCommsDotNet.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    class Logger
    {
        ILogger logger;
        public Logger(string logFile)
        {
            string logFileName = logFile+ "_" + NetworkComms.NetworkIdentifier + ".txt";
            logger = new LiteLogger(LiteLogger.LogMode.ConsoleAndLogFile, logFileName);
            NetworkComms.EnableLogging(logger);
        }
    }
}
