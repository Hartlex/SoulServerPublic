using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KaymakNetwork;
using KaymakNetwork.Network.Server;
using NetworkCommsDotNet.Connections;
using static SunCommon.PacketStructs;

namespace SunCommon
{
    public static class AuthPackets
    {
        public class S2CHelloPacket : AuthPacket
        {
            readonly byte[] unk1 = new byte[64];
            readonly byte[] encKey = new byte[4];
            public S2CHelloPacket(int encKey) :base(0) //Call base Constructor with protocolID
            {
                this.encKey = BitConverter.GetBytes(encKey);
            }

            public new void Send(Connection connection)
            {
                var sendableBytes = GetSendableBytes(unk1, encKey);
                connection.SendUnmanagedBytes(sendableBytes);
            }
        }

        public class C2SAskConnect : AuthPacket
        {
            public byte[] ProtocolVersion;
            public IPAddress serverAddress;
            public C2SAskConnect(ByteBuffer buffer, Connection connection) : base(01,connection)
            {
                ProtocolVersion = buffer.ReadBlock(3);
                var ipstring = Encoding.ASCII.GetString(buffer.ReadBlock(9));
                serverAddress = IPAddress.Parse(ipstring);
            }
        }

        public class S2CAnsConnect : AuthPacket
        {
            private readonly byte success;
            private byte[] unk1 = {20};     //alway 20 idk why
            public S2CAnsConnect(bool success) : base(02)
            {
                this.success = (byte) (success ? 00 : 01);
            }

            public new void Send(Connection connection)
            {
                var sendableBytes = GetSendableBytes(new[] {success},unk1);
                connection.SendUnmanagedBytes(sendableBytes);
            }
        }

        public class C2SAskLogin : AuthPacket
        {
            private byte[] _unk1;
            public string Username { get; }
            private byte[] _unk2;
            private byte[] _encPassword;
            private byte[] _unk3;
            public string DecPassword { get; }

            public C2SAskLogin(ByteBuffer buffer, Connection connection,sbyte[] key=null) : base(3,connection)
            {
                key = new sbyte[] {00, 00, 00, 00};
                //TODO implement dynamic key;
                _unk1 = buffer.ReadBlock(4);
                //get username without trailing zeros
                var uname = buffer.ReadBlock(50); 
                for (int i = 0; i < uname.Length; i++)
                {
                    if (uname[i] == 0)
                    {
                        byte[] help = new byte[i];
                        Array.Copy(uname, help, i);
                        Username = Encoding.ASCII.GetString(help);
                        break;
                    }
                }

                _unk2 = buffer.ReadBlock(1);
                _encPassword = buffer.ReadBlock(16);
                var decPWBytes = TEA.passwordDecodeSBytes(ByteUtils.ToSbytes(_encPassword), key);
                DecPassword = Encoding.ASCII.GetString(ByteUtils.ToByteArray(decPWBytes));
                _unk3 = buffer.ReadBlock(8);
            }
            
            public override void OnReceive()
            {
                //Todo check for user in DB and check PW
                //dbConnection.SendObject("UserLogin",new[]{_username,_decPassword});

                //bool b = true;
                //var packet = new S2CAnsLogin(b);
                //packet.Send(connection);
            }
        }

        public class S2CAnsLogin : AuthPacket
        {
            private readonly byte success;
            public S2CAnsLogin(bool success) : base(14)
            {
                this.success = (byte)(success ? 00 : 01);
            }
            public new void Send(Connection connection)
            {
                var sendableBytes = GetSendableBytes(new[] { success });
                connection.SendUnmanagedBytes(sendableBytes);
            }
        }

        public class C2SAskForServerList : AuthPacket
        {
            public C2SAskForServerList(Connection connection) : base(15, connection){}

            public override void OnReceive()
            {

            }
        }

        public class S2CServerInfo : AuthPacket
        {
            private byte serverCount;
            private ServerInfo[] serverInfos;
            public S2CServerInfo(params ServerInfo[] serverInfos) : base(17)
            {
                this.serverCount = (byte)serverInfos.Length;
                this.serverInfos = serverInfos;
            }

            public new void Send(Connection connection)
            {
                List<byte> allInfo= new List<byte>();
                foreach (var i in serverInfos)
                {
                    allInfo.AddRange(i.getBytes());
                }

                var sendableBytes = GetSendableBytes(new []{serverCount},allInfo.ToArray());
                connection.SendUnmanagedBytes(sendableBytes);
            }
        }

        public class S2CChannelInfo : AuthPacket
        {
            private byte channelCount;
            private ChannelInfo[] channelInfos;

            public S2CChannelInfo(params ChannelInfo[] channelInfos) : base(18)
            {
                this.channelCount = (byte)channelInfos.Length;
                this.channelInfos = channelInfos;
            }

            public new void Send(Connection connection)
            {
                List<byte> allInfo = new List<byte>();
                foreach (var i in channelInfos)
                {
                    allInfo.AddRange(i.getBytes());
                }

                var sendableBytes = GetSendableBytes(new[] {channelCount}, allInfo.ToArray());
                connection.SendUnmanagedBytes(sendableBytes);

            }
        }

        public class C2SAskServerSelect : AuthPacket
        {
            public byte server;
            public byte channel;
            public C2SAskServerSelect(ByteBuffer buffer, Connection connection) : base(19, connection)
            {
                this.Connection = connection;
                server = buffer.ReadByte();
                channel = buffer.ReadByte();
            }

            public override void OnReceive()
            {
                //TODO check if user can enter server
                if (true)
                {
                    int userID = 33;
                    string ip = "127.0.0.1";
                    int port = 8000;
                    var packet = new S2CAnsServerSelect2(userID,ip,port);
                    packet.Send(Connection);
                }
            }
        }

        public class S2CAnsServerSelect : AuthPacket
        {
            private byte[] userID;
            private byte[] unk1 = {
                0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x81, 0x70, 0x42, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x0E, 0x00, 0x07, 0x80, 0xD0, 0xF4, 0x12, 0x00, 0x10, 0x00, 0xC1, 0x03, 0x0E, 0x00
            };
            private byte[] agentIPAddress;
            private byte[] agentPort;

            public S2CAnsServerSelect(int userID, string agentIPAddress, int agentPort) : base(26)
            {
                this.userID = ByteUtils.ToByteArray(userID, 4);
                this.agentIPAddress = ByteUtils.ToByteArray(agentIPAddress, 32);
                this.agentPort = ByteUtils.ToByteArray(agentPort, 5);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(userID, unk1, agentIPAddress, agentPort);
                connection.SendUnmanagedBytes(sb);
            }
        }
        public class S2CAnsServerSelect2 : AuthPacket
        {

            private byte[] userID = {0xf1, 0xa2, 0xcf, 0x78};
            private byte[] unk1 = {
                0x2e, 0x5c, 0x8a, 0xb8, 0xe6, 0x14, 0x42, 0x70, 0x9e, 0xcc, 0xfa, 0x28, 0x56, 0x84, 0xb2, 0xe0, 0x0e,
                0x3c, 0x6a, 0x98, 0xc6, 0xf4, 0x22, 0x50, 0x7e, 0xac, 0xda, 0x08, 0x36, 0x64, 0x92, 0xc0
            };
            private byte[] agentIPAddress;
            private byte[] agentPort;

            private byte[] unk2 =
            {
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x22,0x4e,0x00,
                0x00,0x00,0x72,0x6e,0x38,0x75,0x30,0x30,0x35,0x76,
                0x00
            };
            public S2CAnsServerSelect2(int userID, string agentIPAddress, int agentPort) : base(26)
            {
                //this.userID = ByteUtils.ToByteArray(userID, 4);
                this.agentIPAddress = ByteUtils.ToByteArray(agentIPAddress, 32);
                this.agentPort = ByteUtils.ToByteArray(agentPort, 5);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(userID, unk1, agentIPAddress, agentPort,unk2);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
