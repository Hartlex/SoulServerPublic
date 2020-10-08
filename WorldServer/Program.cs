using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logger  = new Logger("WorldLog.txt");
            NetworkConfig.InitializeNetwork();
            AgentConnection.IniitializeConnection("127.0.0.1",6001);
            while (true)
            {
                ConsoleHandler.ConsoleTick();
            }
        }
    }
}
