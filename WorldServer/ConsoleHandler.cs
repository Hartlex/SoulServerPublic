using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer
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

            }
        }
    }
}
