using KaymakNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    internal static class PacketParser
    {

        public static void ParseUnmanagedPacket(byte[] packet)
        {
            ByteBuffer buffer = new ByteBuffer(packet);
            var packetSize = buffer.ReadBlock(2);
            var PacketID = buffer.ReadBlock(1);
            var parsedPacket = new IPPacket(buffer);
            
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
