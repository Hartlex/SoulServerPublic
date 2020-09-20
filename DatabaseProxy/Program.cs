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
            //DatabaseFunctions.CreateCharacter(2, "Nightmore1", 1, 1, 1, 1,out var character);
            //DatabaseFunctions.CreateCharacter(2, "Nightmore2", 1, 1, 1, 1, out var character2);
            DatabaseFunctions.getAllCharacters(2,out var c);
            //AgentPackets.CreateCharacterPacket("2","Nightmore","1","1","1","1");
            //AgentPackets.CreateCharacterPacket("2","Nightmore2","1","1","1","1");
            NetworkConfig.InitialiseNetwork();
            while (true)
            {
                //Stuff like ReadLine
                ConsoleHandler.ConsoleTick();
            }
        }
    }
}
