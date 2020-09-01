using NetworkCommsDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    static class ConsoleHandler
    {
        public static void ConsoleTick()
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "send":
                    foreach(var conn in NetworkComms.GetExistingConnection( NetworkCommsDotNet.Connections.ApplicationLayerProtocolStatus.Disabled))
                    {
                        conn.SendUnmanagedBytes(ServerPackets.HelloPacket());
                    }
                    break;
            }
        }
    }
}
