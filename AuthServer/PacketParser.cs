using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;

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
                Console.WriteLine("Packet from "+connection.ConnectionInfo.RemoteEndPoint +" with ID: " + packetID + "|" + protocolID+" succesfully received and parsed");
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
        private static void LogPacketRecieved(int packetID,int protocolID,ByteBuffer buffer,SunPacket packet)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now + ": Packet(" + packetID + "|" + protocolID + ") with bytes: ");
            foreach(byte b in buffer.Data)
            {
                sb.Append((int)b+"|");
            }
            var dir = Directory.CreateDirectory(Environment.CurrentDirectory + "\\Log\\" + packet.ToString());
            File.AppendAllText(dir.FullName+"\\Log.txt", sb.ToString()+Environment.NewLine);
        }

        private static bool FindPacket(int packetID,int protocolID,ByteBuffer buffer,Connection connection)
        {
            if (!AuthPacketProcessors.FindPacketAction((PacketCategory) packetID, protocolID, out var action))
                return false;
            action(buffer, connection);
            return true;
            //var identifier = int.Parse(packetID.ToString() + protocolID.ToString());
            //switch (identifier)
            //{
            //    //TODO REMOVE THIS FKCN SWITCH STATEMENT
            //    case 511:
            //        var c2SAskConnect = new AuthPackets.C2SAskConnect(buffer,connection);
            //        LogPacketRecieved(packetID,protocolID,buffer, c2SAskConnect);
            //        c2SAskConnect.OnReceive();
            //        return true;
            //    case 513:
            //        var C2SAskLogin = new AuthPackets.C2SAskLogin(buffer, connection);
            //        LogPacketRecieved(packetID, protocolID, buffer, C2SAskLogin);
            //        C2SAskLogin.OnReceive();
            //        DBConnection.connection.SendObject("UserLogin",new[]{C2SAskLogin.Username,C2SAskLogin.DecPassword,CCM.getConnectionGUID(connection).ToString()});
            //        return true;
            //    case 5115:
            //        var C2SAskForServerList = new AuthPackets.C2SAskForServerList(connection);
            //        LogPacketRecieved(packetID, protocolID, buffer, C2SAskForServerList);
            //        C2SAskForServerList.OnReceive();
            //        return true;
            //    case 5119:
            //        var C2SAskServerSelect = new AuthPackets.C2SAskServerSelect(buffer, connection);
            //        LogPacketRecieved(packetID, protocolID, buffer, C2SAskServerSelect);
            //        C2SAskServerSelect.OnReceive();
            //        return true;

            //}
            //return false;
        }

        public static bool UserLogin(string username, string password)
        {
            return true;
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
