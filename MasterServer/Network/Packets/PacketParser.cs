using System;
using System.IO;
using System.Text;
using KaymakNetwork;
using MasterServer.Properties;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace MasterServer.Network.Packets
{
    internal static class PacketParser
    {
        public static void ParseUnmanagedPacket(byte[] packet, Connection connection)
        {
            ByteBuffer buffer = new ByteBuffer(packet);                 //Convert to Buffer
            var packetSize = buffer.ReadBlock(2);                       //ReadPacketSize
            var packetID = (int)buffer.ReadByte();
            var protocolID = (int)buffer.ReadByte();
            if (FindPacket(packetID, protocolID, buffer, connection, out var action))
            {
                Console.WriteLine("Packet from "+ connection.ConnectionInfo.RemoteEndPoint+": "+action.Method.Name);
                //Console.WriteLine("Packet from " + connection.ConnectionInfo.RemoteEndPoint + " with ID: " + packetID + "|" + protocolID + " succesfully received and parsed");
                return;
            }
            Console.WriteLine("\nReceived unmanaged byte[] ");
            var sb = new StringBuilder();
            for (int i = 0; i < packet.Length; i++)
                sb.Append(packet[i] + "|");
            Console.WriteLine(sb.ToString());
        }
        private static bool FindPacket(int packetID, int protocolID, ByteBuffer buffer, Connection connection, out Action<ByteBuffer,Connection> action)
        {
            if (!PacketProcessor.FindPacketAction((PacketCategory)packetID, protocolID, out action))
                return false;
            LogPacketRecieved(packetID, protocolID, buffer, action.Method.Name);
            action(buffer, connection);
            return true;
        }
        public static void LogPacketRecieved(int packetID, int protocolID, ByteBuffer buffer, string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now + ": Packet(" + packetID + "|" + protocolID + ") with bytes: ");
            foreach (byte b in buffer.Data)
            {
                sb.Append((int)b + "|");
            }
            var dir = Directory.CreateDirectory(Environment.CurrentDirectory + "\\Log\\" + name);
            File.AppendAllText(dir.FullName + "\\Log.txt", sb.ToString() + Environment.NewLine);
        }

        public static void Initialize()
        {
            Console.WriteLine(Resources.PacketParser_Initialize_Load);
            PacketProcessor.Initialize();
            Console.WriteLine(Resources.PacketParser_Initialize_Success);
        }

        public static void Unload()
        {
            PacketProcessor.AllPackets.Clear();
            Console.WriteLine(Resources.PacketParser_Unload_Success);
        }
    }
}
