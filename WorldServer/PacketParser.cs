using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace WorldServer
{
    static class PacketParser
    {
        public static void ParseUnmanagedPacket(byte[] packet, Connection connection)
        {
            ByteBuffer buffer = new ByteBuffer(packet);                 //Convert to Buffer
            var packetSize = buffer.ReadBlock(2);                       //ReadPacketSize
            var packetID = (int)buffer.ReadByte();
            var protocolID = (int)buffer.ReadByte();
            if (FindPacket(packetID, protocolID, buffer, connection))
            {
                Console.WriteLine("Packet from " + connection.ConnectionInfo.RemoteEndPoint + " with ID: " + packetID + "|" + protocolID + " succesfully received and parsed");
                return;
            }
            Console.WriteLine("\nReceived unmanaged byte[] ");

            for (int i = 0; i < packet.Length; i++)
                Console.Write(packet[i].ToString() + "|");
        }

        private static bool FindPacket(int packetId, int protocolId, ByteBuffer buffer, Connection connection)
        {
            return false;
        }
    }
}
