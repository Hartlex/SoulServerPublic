using KaymakNetwork;
using NetworkCommsDotNet.Connections;
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
    
    internal class IPPacket : SunPacket
    {
        public byte[] ProtocolVersion; // 3 Bytes
        public IPAddress serverAddress; // 9 Bytes
        public IPPacket(ByteBuffer buffer, Connection connection)
        {
            this.connection = connection;
            ProtocolVersion = buffer.ReadBlock(3);
            var ipString = Encoding.ASCII.GetString(buffer.ReadBlock(9));
            serverAddress = IPAddress.Parse(ipString);
           
        }
        public override void onReceive()
        {
            connection.SendUnmanagedBytes(ServerPackets.ClientAckPacket());
        }
        
    }
    internal class LoginPacket : SunPacket
    {
        
        public LoginPacket(ByteBuffer buffer, Connection connection)
        {
            this.connection = connection;
        }
        public override void onReceive()
        {
            connection.SendUnmanagedBytes(ServerPackets.LoginSuccessfull());
        }
    }
    internal class AskForServerList : SunPacket
    {
        public AskForServerList(ByteBuffer buffer, Connection connection)
        {
            this.connection = connection;
        }
        public override void onReceive()
        {
            connection.SendUnmanagedBytes(ServerPackets.ServerInfo());
            connection.SendUnmanagedBytes(ServerPackets.ChannelInfo());
        }
    }
    internal class SelectServerAndChannel : SunPacket
    {
        public int server;
        public int channel;
        public SelectServerAndChannel(ByteBuffer buffer, Connection connection)
        {
            this.connection = connection;
            server = buffer.ReadByte();
            channel = buffer.ReadByte();
        }
        public override void onReceive()
        {
            connection.SendUnmanagedBytes(ServerPackets.ConfirmServerSelect());
        }
    }
}
