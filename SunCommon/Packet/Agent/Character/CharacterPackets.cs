using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using static SunCommon.PacketStructs;

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

            public C2SAskCreateCharacter(ByteBuffer buffer, Connection connection) : base(111, connection)
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

            public C2SAskDublicateNameCheck(ByteBuffer buffer, Connection connection) : base(81, connection)
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
                var sb = GetSendableBytes(BitConverter.GetBytes(code));
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CAnsCreateCharacter : CharacterPacket
        {
            public CharacterInfo character;
            public byte[] bytes;

            public S2CAnsCreateCharacter(byte[] bytes, Connection connection) : base(226, connection)
            {
                this.bytes = bytes;
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(bytes);
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
                unk1 = buffer.ReadBlock(buffer.Data.Length - buffer.Head);
            }
        }

        public class S2CAnsDeleteCharacter : CharacterPacket
        {
            public S2CAnsDeleteCharacter(Connection connection) : base(7, connection)
            {
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes();
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
                var sb = GetSendableBytes(BitConverter.GetBytes(errorCode));
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CCharacterInfo : CharacterPacket
        {
            private byte[] unk0;
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
            private byte[] strength;
            private byte[] vitality;
            private byte[] dexterity;
            private byte[] intelligence;
            private byte[] spirit;
            private byte[] skillStat1;
            private byte[] skillStat2;
            private byte[] gmAndStateInfo;
            private GmAndStateInfo gmASInfo;
            private byte[] playLimitedTime;
            private byte[] invisOption;
            private byte[] unk3;
            private byte[] unk4;
            private byte[] unk5;
            private byte[] unk6;
            private byte[] unk7;
            private byte[] unk8;
            private byte[] pkState;
            private byte[] inventoryExpand;
            private byte[] inventoryInfo;
            private byte[] unkSize;
            private byte[] sdShield;
            private byte[] unk9;

            public S2CCharacterInfo(Entities.Character character) : base(42)
            {
                exp = BitConverter.GetBytes(character.Experience);
                remainSkillPoint = BitConverter.GetBytes(character.RemainSkill);
                remainStatPoint = BitConverter.GetBytes(character.RemainStat);
                money = BitConverter.GetBytes(character.Inventory.Money);
                selectedStyle = BitConverter.GetBytes((short)character.SelectedStyle);//maybe selected style but 2 bytes maybe ushort
                maxHp = new byte[] {0, 0}; //max hp calculated by client
                hp = BitConverter.GetBytes((ushort) character.Hp);
                maxMp = new byte[] {0, 0}; //max mp calculated by client
                mp = BitConverter.GetBytes((ushort)character.Mp);
                moveSpeed = BitConverter.GetBytes((short) 150); //add movementSpeed as attribute as short maybe? calculated by client
                attackSpeed = BitConverter.GetBytes((short) 152); //Add AttackSpeed as Short calculated by client
                stateTime = BitConverter.GetBytes(character.StateTime);
                titleId = new byte[16]; //TODO make title a byte depending on next bytes
                unk1 = new byte[] {00, 01};
                titleTime = BitConverter.GetBytes(character.TitleTime); // dont know if correct
                inventoryLock = new[] {character.Inventory.InventoryLock};
                unk2 = new byte[] {00};
                strength = BitConverter.GetBytes((short)character.Strength);
                vitality = BitConverter.GetBytes((short)character.Vitality);
                dexterity = BitConverter.GetBytes((short)character.Dexterity);//TODO test if order is correct
                intelligence = BitConverter.GetBytes((short)character.Intelligence);
                spirit = BitConverter.GetBytes((short)character.Spirit);
                skillStat1 = BitConverter.GetBytes((short)character.SkillStat1);
                skillStat2 = BitConverter.GetBytes((short) character.SkillStat2);
                //gmASInfo.GmGrade = 8;
                //gmASInfo.PcBangUser = 0;
                //gmASInfo.Condition = 0;
                //gmASInfo.PkState = character.PkState;
                //gmASInfo.CharState = character.CharState;
                //gmAndStateInfo = gmASInfo.getValue();
                gmAndStateInfo = new byte[]{00,00};
                playLimitedTime = BitConverter.GetBytes((int)character.PlayLimitedTime); //TODO check data types
                invisOption = BitConverter.GetBytes(character.InvisibleOpt);


                unk8 = new byte[]
                {
                    0x00, 0x00, 0x00, 0x00, //unk dword maybe guild guid
                    0x00, //? guild ID

                    00, 00, 00, 00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 00, 00, 00, 00, //guildname?

                    0x00,
                    0x00,

                    0x01, 0x00,//has guild or guild id
                    0x01,   //pkstate 1 orange 2 red
                    0x03,   //inv Expand

                    0x00, 0x00, 0x00, 0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00,
                    0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    //0x53, 0x00,
                    //0x10, 0x00, // Sd Shield
                };

                unk9= BitConverter.GetBytes((ushort)51);
                sdShield = BitConverter.GetBytes((short)1000);
                //unkSize = BitConverter.GetBytes((short)(5 * character.Inventory.InventoryItem[0]+1));
                //unkSize = BitConverter.GetBytes((short) 11); //num of slots*5 +1
                //inventoryInfo= new byte[]
                //{
                //    02,
                //    0,161,15,1,100,
                //    1,162,15,2,0
                //};
                inventoryInfo = new InventoryTotalInfo(
                    character.Inventory.invSlotsInfo,
                    character.Inventory.tempInventory).ToBytes();
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(
                    exp,
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
                    strength,
                    vitality,
                    dexterity,
                    intelligence,
                    spirit,
                    skillStat1,
                    skillStat2,
                    gmAndStateInfo,
                    playLimitedTime,
                    invisOption,
                    //unk3, unk4, unk5, unk6, inventoryExpand,
                    //unk7,
                    //pkState,
                    unk8,
                    unk9,
                    sdShield,
                    //unkSize,
                    inventoryInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CSkillInfo : CharacterPacket
        {
            private byte[] unk1;

            public S2CSkillInfo(Entities.Character character) : base(159)
            {
                unk1 = new byte[] {0x01, 0x00, 0xe1, 0x2e};
                //TODO probalby the skill byte array
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(unk1);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CQuickInfo : CharacterPacket
        {
            private byte[] quickInfo;

            public S2CQuickInfo(Entities.Character character) : base(190)
            {
                quickInfo = character.Quick;
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(quickInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CStyleInfo : CharacterPacket
        {
            private byte[] styleInfo;

            public S2CStyleInfo(Entities.Character character) : base(193)
            {
                styleInfo = new byte[]
                {
                    0x21, 0x00, 0x00, 0x00, 0x00, 0x0f,
                    0x00
                };
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(styleInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class S2CStatePacket : CharacterPacket
        {
            private byte count;
            private List<StateSlotInfo> slots;

            public S2CStatePacket(Entities.Character character) : base(219)
            {
                count = 2; //why only 2
                slots = new List<StateSlotInfo>(count);
                slots.Add(new StateSlotInfo(new byte[] {0x01, 0x00, 0xe8, 0x03, 0x00, 0x00}));
                slots.Add(new StateSlotInfo(new byte[] {0x02, 0x00, 0xe8, 0x03, 0x00, 0x00}));
            }

            public new void Send(Connection connection)
            {
                var slotbytes = new List<byte>();
                slotbytes.Add(count);
                foreach (var slot in slots)
                {
                    slotbytes.AddRange(slot.ToBytes());
                }

                var sb = GetSendableBytes(slotbytes.ToArray());
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class C2SAskSelectPlayer : CharacterPacket
        {
            public uint objectKey;
            public byte[] unkKey;

            public C2SAskSelectPlayer(ByteBuffer buffer) : base(51)
            {
                this.objectKey = buffer.ReadUInt32();
                this.unkKey = buffer.ReadBlock(4);
            }
        }

        public class S2CAnsSelectPlayer : CharacterPacket
        {
            private byte[] objKey;
            private byte[] hp;
            private byte[] maxHp;
            private byte[] level;

            public S2CAnsSelectPlayer(uint objKey, Single hp, Single maxHp, short level) : base(51)
            {
                this.objKey = BitConverter.GetBytes(objKey);
                this.hp = BitConverter.GetBytes(hp);
                this.maxHp = BitConverter.GetBytes(maxHp);
                this.level = BitConverter.GetBytes(level);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(objKey, hp, maxHp, level);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}