using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public class S2CMonsterEnter : SyncPacket
        {
            private byte[] objKey;
            private byte[] monsterId;
            private byte[] position;
            private byte[] hp;
            private byte[] maxhp;
            private byte[] moveSpeedRatio;
            private byte[] attackSpeedRatio;
            private byte[] unk1;

            public S2CMonsterEnter(uint objKey, ushort monsterId, SunVector position, uint hp,uint maxHp, ushort moveSpeedratio,
                ushort attackSpeedRatio, ushort unk1) : base(174)
            {
                this.objKey = BitConverter.GetBytes(objKey);
                this.monsterId = BitConverter.GetBytes(monsterId);
                this.position = position.GetBytes();
                this.hp = BitConverter.GetBytes(hp);
                this.maxhp = BitConverter.GetBytes(maxHp);
                this.moveSpeedRatio = BitConverter.GetBytes(moveSpeedratio);
                this.attackSpeedRatio = BitConverter.GetBytes(attackSpeedRatio);
                this.unk1 = BitConverter.GetBytes(unk1);


            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(objKey, monsterId, position, hp, maxhp,moveSpeedRatio, attackSpeedRatio,unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }
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
                var sb = GetSendableBytes(position.GetBytes(), unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CAnsGuildInfo : SyncPacket
        {
            private byte[] unk1;
            public S2CAnsGuildInfo(Character character) : base(234)
            {
                unk1 = new byte[] {
                    //0x01, 0x03, 0x00, 0x00, 0x00, 0xb8, 0x5c, 0x00, 0x00, 0xbc,
                    //0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc7, 0xe9,
                    //0xd2, 0xe5, 0xd3, 0xc0, 0xba, 0xe3, 0x00, 0x00, 0x00,
                    //0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x04, 0x00, 0x00

                };
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(unk1);
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
                    //0x01,
                    //0x04, 0x00, 0x00, 0x00,
                    //0x09,
                    //0x00, 0x89, 0x00, 0x0c,
                    //0x00,
                    //0x01, 0xb5,
                    //0xe8, 0x0c, 0x00, 0x02,
                    //0xb6,
                    //0xe8, 0x0c, 0x00, 0x03,
                    //0xb7,
                    //0xe8, 0x0c, 0x00, 0x04,
                    //0xb8,
                    //0xe8, 0x0c, 0x00, 0x05,
                    //0xb9,
                    //0xe8, 0x0c, 0x00, 0x06,
                    //0xba,
                    //0xe8, 0x0c, 0x00, 0x07
                };
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class C2SAskJumpMovePacket : SyncPacket
        {

            public C2SAskJumpMovePacket(ByteBuffer buffer) : base(115)
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

        public class C2SSyncNewPositionAfterJump : SyncPacket
        {
            public SunVector pos;
            public C2SSyncNewPositionAfterJump(ByteBuffer buffer) : base(69)
            {
                pos = new SunVector(buffer.ReadBlock(12));
            }
        }

        public class C2SSyncMoveStop : SyncPacket
        {
            public SunVector pos;
            public C2SSyncMoveStop(ByteBuffer buffer) : base(123)
            {
                pos = new SunVector(buffer.ReadBlock(12));
            }
        }

        public class S2CSyncAllPlayers : SyncPacket
        {
            private byte count;
            private byte[] unk1;
            private byte[] name;
            private byte[] pos;
            private byte[] slotCode;
            private byte[] unk2;
            private byte[] unk3;

            public S2CSyncAllPlayers(byte count) : base(249)
            {
                this.count = count;
                unk1 = new byte[]
                {
                    00,
                    99, 00,
                    200, 00,
                    100, 00,
                    10, 00,
                    16
                };
                name = ByteUtils.ToByteArray("Night2", 16);
                pos = new SunVector(-53f,-32f,-23f).GetBytes();
                slotCode = BitConverter.GetBytes((ushort) 0);
                unk2 = BitConverter.GetBytes(0x14880500);
                unk3 = new byte[]
                {
                    0,0,0,0,
                    1,
                    42,
                    70,
                    3,
                    0,0,0,0,0
                };

            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(new[] {count},unk1,name,pos,slotCode,unk2,unk3);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class C2SSyncMapTile : SyncPacket
        {
            public int MapTileId;
            public int MapId; 
            public C2SSyncMapTile(ByteBuffer buffer) : base(96)
            {
                MapTileId = buffer.ReadInt32();
                MapId = buffer.ReadInt32();
            }
        }

        public class S2CItemEnter : SyncPacket
        {
            private byte[] fromMonster;
            private byte[] objKey;
            private byte[] owner;
            private byte[] itemType;
            private byte[] heimAmount;
            private byte[] unk1;
            private byte[] item;
            private byte[] pos;
            private byte[] unk2;
            public S2CItemEnter(uint fromMonster,uint objKey, uint owner, byte itemType, uint heimAmount, uint unk1, ItemInfo item,
                SunVector pos) : base(93)
            {
                this.fromMonster = BitConverter.GetBytes(fromMonster);
                this.objKey = BitConverter.GetBytes(objKey);
                this.owner = BitConverter.GetBytes(owner);
                this.itemType = new[] {itemType};
                this.heimAmount = BitConverter.GetBytes(heimAmount);
                this.unk1 = BitConverter.GetBytes(unk1);
                this.pos = pos.GetBytes();

                var x = new ItemInfoX(2);
                x.durAmount = 10;
                x.Serial = 10;

                x.Devine = 1;
                x.Rank = 2;
                x.RankD = 10;
                x.Enchant = 10;
                this.item = x.GetBytes();
                unk2 = new byte[5];

            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(fromMonster,objKey, owner, itemType, heimAmount, unk1, item,unk2, pos);
                connection.SendUnmanagedBytes(sb);
            }
        }

    }
}
