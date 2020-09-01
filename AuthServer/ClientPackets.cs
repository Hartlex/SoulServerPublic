using KaymakNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    static class ClientPackets
    {

    }
    internal class IPPacket
    {
        public int unknown1; //1 Byte
        public byte[] ProtocolVersion; // 3 Bytes
        public IPAddress serverAddress; // 9 Bytes
        public IPPacket(ByteBuffer buffer)
        {
            unknown1 = buffer.ReadByte();
            ProtocolVersion = buffer.ReadBlock(3);
            var ipString = Encoding.ASCII.GetString(buffer.ReadBlock(9));
            serverAddress = IPAddress.Parse(ipString);
           
        }
    }
}
