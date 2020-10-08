using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public static class ConnectionPackets
    {
        public class C2SAskEnterCharSelect :ConnectionPacket
        {
            private byte[] unk1;
            public int userID;
            public string username;
            public C2SAskEnterCharSelect(ByteBuffer buffer, NetworkCommsDotNet.Connections.Connection connection) :
                base(118, connection)
            {
                //TODO figure out packet struct
                unk1 = buffer.ReadBlock(2);
                userID= BitConverter.ToInt32(buffer.ReadBlock(5), 0);
                var uname = buffer.ReadBlock(50);
                for (int i = 0; i < uname.Length; i++)
                {
                    if (uname[i] == 0)
                    {
                        byte[] help = new byte[i];
                        Array.Copy(uname, help, i);
                        username = Encoding.ASCII.GetString(help);
                        break;
                    }
                }
            }

            public override void OnReceive()
            {
                ////TODO link userIDs
                //var userID = ByteUtils.ToByteArray(33, 4);
                //var packet = new S2CAnsEnterCharSelect(userID);
                //packet.Send(Connection);
            }
        }
        public class S2CAnsEnterCharSelect : ConnectionPacket
        {
            private byte[] userID;
            private byte charCount;
            private byte unk2 = 00;
            private byte[] characterInfobytes;
            public S2CAnsEnterCharSelect(byte[] characterInfobytes) : base(152)
            {

                //this.userID = ByteUtils.ToByteArray(userID, 4);
                //this.charCount = (byte)charCount;
                //this.unk2 = this.charCount;
                this.characterInfobytes = characterInfobytes;
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(characterInfobytes);
                connection.SendUnmanagedBytes(sb);
            }
        }
        public class C2SAskEnterGame : ConnectionPacket
        {
            public byte unk1;
            public byte[] charSlotBytes;
            public byte charSlot;
            public C2SAskEnterGame(ByteBuffer buffer, Connection connection) : base(31, connection)
            {
                unk1 = buffer.ReadByte();
                charSlotBytes = buffer.ReadBlock(2);
                charSlot= (byte) (BitConverter.ToInt16(charSlotBytes, 0)/128);
            }

        }
        public class S2CAnsEnterGame : ConnectionPacket
        {
            public byte[] characterID;
            public byte[] unk1 = {0,0,0,0};
            public S2CAnsEnterGame(int characterID) : base(131)
            {
                this.characterID = BitConverter.GetBytes(characterID);
                
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(characterID);
                connection.SendUnmanagedBytes(sb);
            }
        }
        public class C2SAskWorldPrepare : ConnectionPacket
        {
            public C2SAskWorldPrepare() : base(223)
            {

            }

        }

        public class S2CAnsWorldPrepare : ConnectionPacket
        {
            private byte[] ip;
            private byte[] port;

            public S2CAnsWorldPrepare(string worldServerIp, int worldServerPort) : base(21)
            {
                ip = ByteUtils.ToByteArray(worldServerIp,32);
                port = ByteUtils.ToByteArray(worldServerPort, 5);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(ip, port);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
