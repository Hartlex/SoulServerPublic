using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    internal static class PacketParser
    {
        

        public static void ParseUnmanagedPacket(byte[] packet, Connection connection)
        {
            ByteBuffer buffer = new ByteBuffer(packet);                 //Convert to Buffer
            var packetSize = buffer.ReadBlock(2);                       //ReadPacketSize
            var packetID = (int)buffer.ReadByte();                      
            var protocolID = (int)buffer.ReadByte();
            if(FindPacket(packetID, protocolID, buffer, connection))
            {
                Console.WriteLine("Packet from with ID: " + packetID + "|" + protocolID+" succesfully received and parsed");
                return;
            }
            Console.WriteLine("\nReceived unmanaged byte[] ");

            for (int i = 0; i < packet.Length; i++)
                Console.Write(packet[i].ToString() + "|");
        }

        public static char[] getASCIIArray(byte[] packet)
        {
            char[] charArray = new char[packet.Length];
            for(int i = 0; i < packet.Length; i++)
            {
                charArray[i] = (char) packet[i];
            }
            return charArray;
        }
        

        private static bool FindPacket(int packetID,int protocolID,ByteBuffer buffer,Connection connection)
        {
            var identifier = int.Parse(packetID.ToString() + protocolID.ToString());
            SunPacket packet;
            switch (identifier)
            {
                case 511:
                    packet = new IPPacket(buffer, connection);
                    packet.onReceive();
                    return true;
                case 513:
                    packet = new LoginPacket(buffer, connection);
                    packet.onReceive();
                    return true;
                case 5115:
                    packet = new AskForServerList(buffer, connection);
                    packet.onReceive();
                    return true;
                case 5119:
                    packet = new SelectServerAndChannel(buffer, connection);
                    packet.onReceive();
                    return true;
                
            }
            return false;
        }

        #region GetStringMethods
        public static string GetEndianString(byte[] packet)
        {
            return Encoding.BigEndianUnicode.GetString(packet);
        }
        public static string GetASCIIString(byte[] packet)
        {
            return Encoding.ASCII.GetString(packet);
        }
        public static string GetUnicodeString(byte[] packet)
        {
            return Encoding.Unicode.GetString(packet);
        }
        public static string GetUTF32String(byte[] packet)
        {
            return Encoding.UTF32.GetString(packet);
        }
        #endregion
    }
}
