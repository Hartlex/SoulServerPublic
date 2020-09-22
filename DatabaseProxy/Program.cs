using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProxy
{
    class Program
    {

        static void Main(string[] args)
        {

            //DatabaseFunctions.GetFullCharacter(2, 0, out var fullCharBytes);
            NetworkConfig.InitialiseNetwork();
            while (true)
            {
                //Stuff like ReadLine
                ConsoleHandler.ConsoleTick();
            }
        }
    }
}
