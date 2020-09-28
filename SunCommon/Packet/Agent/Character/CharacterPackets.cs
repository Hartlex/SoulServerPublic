using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Character
{
    public static class CharacterPackets
    {
        public class C2SAskCreateCharacter : CharacterPacket
        {
            public byte[] unk1;
            public int ClassCode;
            public byte[] unk2;
            public string CharName;
            public int HeightCode;
            public int FaceCode;
            public int HairCode;
            public C2SAskCreateCharacter(ByteBuffer buffer,Connection connection) : base(111, connection)
            {
                unk1 = buffer.ReadBlock(15);
                ClassCode = buffer.ReadByte();
                unk2 = buffer.ReadBlock(4);
                var charName = buffer.ReadBlock(16);

                for (int i = 0; i < charName.Length; i++)
                {
                    if (charName[i] == 0)
                    {
                        byte[] help = new byte[i];
                        Array.Copy(charName, help, i);
                        CharName = Encoding.ASCII.GetString(help);
                        break;
                    }
                }
                HeightCode = buffer.ReadByte();
                FaceCode = buffer.ReadByte();
                HairCode = buffer.ReadByte();
            }
        }

        public class C2SAskDublicateNameCheck : CharacterPacket
        {
            private string name;
            private byte unk1;
            public C2SAskDublicateNameCheck(ByteBuffer buffer,Connection connection) : base(81, connection)
            {
                var b =buffer.ReadBlock(16);
                name = Encoding.ASCII.GetString(b);
                unk1 = buffer.ReadByte();
            }

            public override void OnReceive()
            {
                //TODO check char name in DB
            }
        }

        public class S2CAnsCreateCharacter : CharacterPacket
        {
            public PacketStructs.CharacterInfo character;
            public byte[] bytes;
            public S2CAnsCreateCharacter(byte[] bytes, Connection connection) : base(226, connection)
            {
                this.bytes = bytes;
            }

            public new void Send(Connection connection)
            {
                var sb =getSendableBytes(bytes);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
