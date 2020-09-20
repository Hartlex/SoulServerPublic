using NetworkCommsDotNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Logger log = new Logger("AuthLog");
            Console.WriteLine("Connecting to DBServer");
            DBConnection.IniitializeConnection("127.0.0.1", 7000);
            AuthPacketProcessors.Initialize();
            while (!DBConnection.isConnected)
            {
                //Console.Write(".");
            }
            //Start Logging

            //Initialize Network
            NetworkConfig.InitialiseNetwork();

            //Keep Programm running
            while (true)
            {
                //Stuff like ReadLine
                ConsoleHandler.ConsoleTick();
            }
        }
    }
}
