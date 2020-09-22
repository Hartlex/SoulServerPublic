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
            public string name;
            private byte unk1;
            public C2SAskDublicateNameCheck(ByteBuffer buffer,Connection connection) : base(81, connection)
            {

                var charName = buffer.ReadBlock(16);

                for (int i = 0; i < charName.Length; i++)
                {
                    if (charName[i] == 0)
                    {
                        byte[] help = new byte[i];
                        Array.Copy(charName, help, i);
                        name = Encoding.ASCII.GetString(help);
                        break;
                    }
                }

                unk1 = buffer.ReadByte();
            }

            public override void OnReceive()
            {
                //TODO check char name in DB
            }
        }

        public class S2CAnsDuplicateNameCheck : CharacterPacket
        {
            private int code;
            public S2CAnsDuplicateNameCheck(int code, Connection connection) : base(45, connection)
            {
                this.code = code;
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(BitConverter.GetBytes(code));
                connection.SendUnmanagedBytes(sb);
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

        public class C2SAskDeleteCharacter : CharacterPacket
        {
            public byte charSlot;
            public byte[] unk1; 
            public C2SAskDeleteCharacter(ByteBuffer buffer, Connection connection) : base(137, connection)
            {
                charSlot = buffer.ReadByte();
                unk1 = buffer.ReadBlock(buffer.Data.Length-buffer.Head);
            }
        }

        public class S2CAnsDeleteCharacter : CharacterPacket
        {
            public S2CAnsDeleteCharacter(Connection connection) : base(7, connection)
            {

            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes();
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CErrCharacterPacket : CharacterPacket
        {
            private int errorCode;
            public S2CErrCharacterPacket(int errorCode, Connection connection) : base(113, connection)
            {
                this.errorCode = errorCode;
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(BitConverter.GetBytes(errorCode));
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
