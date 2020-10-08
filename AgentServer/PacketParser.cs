using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Packet.Agent.Character;


namespace AgentServer
{
    internal class PacketParser
    {
        internal static void ParseUnmanagedPacket(byte[] packet, Connection connection)
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

        private static bool FindPacket(int packetID, int protocolID, ByteBuffer buffer, Connection connection)
        {
            if (!AgentPacketProcessors.FindPacketAction((PacketCategory)packetID, protocolID, out var action))
                return false;
            SunCommon.PacketParser.LogPacketRecieved(packetID,protocolID,buffer,action.Method.Name);
            action(buffer, connection);
            return true;

        }
    }
}