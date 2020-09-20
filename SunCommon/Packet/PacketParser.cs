using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;

namespace SunCommon
{
    static class PacketParser
    {

        public static void ParseIncomingBytes(byte[] packetBytes)
        {

        }

        private static void ProcessUnknownPacket(byte[] packetBytes)
        {
            Console.WriteLine("\nReceived unmanaged byte[] ");

            for (int i = 0; i < packetBytes.Length; i++)
                Console.Write(packetBytes[i] + "|");
        }
    }
}
