using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using SunCommon.Entities;
using static SunCommon.PacketStructs;

namespace SunCommon
{
    public static class SyncPackets
    {
        public class C2SAskEnterWorld: SyncPacket
        {
            private byte[] unk1;
            public C2SAskEnterWorld(ByteBuffer buffer) : base(141)
            {
                unk1 = buffer.ReadBlock(16);
            }

        }

        public class S2CAnsPlayerPositionInfo : SyncPacket
        {
            private SunVector position;
            private byte[] unk1;
            public S2CAnsPlayerPositionInfo(Character character) : base(31)
            {
                position = new SunVector(
                    character.CharacterPosition.LocationX,
                    character.CharacterPosition.LocationY,
                    character.CharacterPosition.LocationZ
                );
                unk1 = new byte[]{00,00};
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(position.GetBytes(), unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CAnsGuildInfo : SyncPacket
        {
            private byte[] unk1;
            public S2CAnsGuildInfo(Character character) : base(234)
            {
                unk1 = new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0xb8, 0x5c, 0x00, 0x00, 0xbc,
                    0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc7, 0xe9,
                    0xd2, 0xe5, 0xd3, 0xc0, 0xba, 0xe3, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x04, 0x00, 0x00};
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CAnsEquipInfo : SyncPacket
        {
            private byte[] unk1;

            public S2CAnsEquipInfo(Character character) : base(15)
            {
                unk1 = new byte[]
                {
                    0x01,
                    0x04, 0x00, 0x00, 0x00,
                    0x09,
                    0x00, 0x89, 0x00, 0x0c,
                    0x00,
                    0x01, 0xb5,
                    0xe8, 0x0c, 0x00, 0x02,
                    0xb6,
                    0xe8, 0x0c, 0x00, 0x03,
                    0xb7,
                    0xe8, 0x0c, 0x00, 0x04,
                    0xb8,
                    0xe8, 0x0c, 0x00, 0x05,
                    0xb9,
                    0xe8, 0x0c, 0x00, 0x06,
                    0xba,
                    0xe8, 0x0c, 0x00, 0x07
                };
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class C2SAskJumpMovePacket : SyncPacket
        {
            public C2SAskJumpMovePacket() : base(115)
            {

            }

        }

        public class C2SAskKeyboardMovePacket : SyncPacket
        {
            public SunVector currentPosition;
            public byte[] angle;
            public byte[] tileIndex;
            public byte moveState;

            public C2SAskKeyboardMovePacket(ByteBuffer buffer) : base(43)
            {
                currentPosition = new SunVector(buffer.ReadBlock(12));
                angle = buffer.ReadBlock(2);
                tileIndex = buffer.ReadBlock(2);
                moveState = buffer.ReadByte();
            }
        }

        public class C2SAskMouseMove : SyncPacket
        {
            public byte[] unk1;
            public SunVector currentPosition;
            public SunVector destinationPosition;

            public C2SAskMouseMove(ByteBuffer buffer) : base(202)
            {
                unk1 = buffer.ReadBlock(2);
                currentPosition = new SunVector(buffer.ReadBlock(12));
                destinationPosition = new SunVector(buffer.ReadBlock(16));
            }

        }
    }
}
