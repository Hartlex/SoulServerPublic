using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;

namespace AgentServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DBConnection.IniitializeConnection("127.0.0.1",7001);
            //AuthServerConnection.IniitializeConnection("127.0.0.1",8001);
            NetworkConfig.InitializeNetwork();
            AgentPacketProcessors.Initialize();
            while (true)
            {
                ConsoleHandler.ConsoleTick();
            }
        }
    }
}
