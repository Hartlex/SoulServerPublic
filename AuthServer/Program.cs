using NetworkCommsDotNet.Tools;
using System;
using System.Collections.Generic;
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

            //Start Logging
            //Logger log = new Logger("AuthLog");
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
