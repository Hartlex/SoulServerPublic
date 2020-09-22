using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public class S2CCharacterInfo : CharacterPacket
        {
            private byte[] exp;
            private byte[] remainSkillPoint;
            private byte[] remainStatPoint;
            private byte[] money;
            private byte[] selectedStyle;
            private byte[] maxHp;
            private byte[] hp;
            private byte[] maxMp;
            private byte[] mp;
            private byte[] moveSpeed;
            private byte[] attackSpeed;
            private byte[] stateTime;
            private byte[] titleId;
            private byte[] unk1;
            private byte[] titleTime;
            private byte[] inventoryLock;
            private byte[] unk2;
            private byte[] unk3;
            public S2CCharacterInfo(Entities.Character character) : base(42)
            {
                exp = BitConverter.GetBytes(character.Experience);
                remainSkillPoint = BitConverter.GetBytes(character.RemainSkill);
                remainStatPoint = BitConverter.GetBytes(character.RemainStat);
                money = BitConverter.GetBytes(character.Inventory.Money);
                selectedStyle = new byte[]{240,235}; //maybe selected style but 2 bytes maybe ushort
                maxHp = new byte[]{158,8};  //max hp why 2 bytes if it is float/real in db which is 4 bytes
                hp = new byte[]{0x3b,0x01}; //hp
                maxMp = new byte[] {0xc4, 0x03}; //max mp
                mp = new byte[] {0xda, 0x00}; //mp
                moveSpeed = BitConverter.GetBytes((short) 101); //add movementSpeed as attribute as short maybe?
                attackSpeed = BitConverter.GetBytes((short) 152); //Add AttackSpeed as Short
                stateTime = BitConverter.GetBytes(character.StateTime);
                titleId = new byte[]{00}; //TODO make title a byte depending on next bytes
                unk1 = new byte[]{00,00};
                titleTime = BitConverter.GetBytes(character.TitleTime); // dont know if correct
                inventoryLock = new []{character.Inventory.InventoryLock};
                unk2 = new byte[]{00};
                unk3 = new byte[]
                {
                    0x00, 0x00,
                    0x00, 0x00,
                    0x01, 0x01,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00,

                    0x14, 0x00,
                    0x12, 0x00,
                    0x0f, 0x00,
                    0x0e, 0x00,
                    0x0d, 0x00,
                    0x10, 0x00,
                    0x10, 0x00,

                    0x00, 0x00, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0xff,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x53, 0x00, 0x35, 0x00, 0x00, 0x00, 0x00
                };

            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(exp,
                    remainSkillPoint,
                    remainStatPoint,
                    money,
                    selectedStyle,
                    maxHp,
                    hp,
                    maxMp,
                    mp,
                    moveSpeed,
                    attackSpeed,
                    stateTime,
                    titleId,
                    unk1,
                    titleTime,
                    inventoryLock,
                    unk2,
                    unk3);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CSkillInfo : CharacterPacket
        {
            private byte[] unk1;
            public S2CSkillInfo(Entities.Character character) : base(159)
            {
                unk1 = new byte[]{0x01,0x00,0xe1,0x2e};
                //TODO probalby the skill byte array
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CQuickInfo:CharacterPacket
        {
            private byte[] quickInfo;
            public S2CQuickInfo(Entities.Character character) : base(190)
            {
                quickInfo = character.Quick;
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(quickInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CStyleInfo : CharacterPacket
        {
            private byte[] styleInfo;
            public S2CStyleInfo(Entities.Character character) : base(193)
            {
                styleInfo = new byte[]{0x21, 0x00, 0x00, 0x00, 0x00, 0x0f,
                    0x00};
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(styleInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CStatePacket : CharacterPacket
        {
            private byte count;
            private List<PacketStructs.StateSlotInfo> slots;
            public S2CStatePacket(Entities.Character character) : base(219)
            {
                count = 2; //why only 2
                slots = new List<PacketStructs.StateSlotInfo>(count);
                slots.Add(new PacketStructs.StateSlotInfo(new byte[]{0x01,0x00,0xe8,0x03,0x00,0x00}));
                slots.Add(new PacketStructs.StateSlotInfo(new byte[]{0x02,0x00,0xe8,0x03,0x00,0x00}));
            }

            public new void Send(Connection connection)
            {
                var slotbytes = new List<byte>();
                slotbytes.Add(count);
                foreach (var slot in slots)
                {
                    slotbytes.AddRange(slot.ToBytes());
                }

                var sb = getSendableBytes(slotbytes.ToArray());
                connection.SendUnmanagedBytes(sb);
            }
        }


    }
}
