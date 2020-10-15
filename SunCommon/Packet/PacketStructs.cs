using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections.Bluetooth;
using Newtonsoft.Json.Schema;
using SunCommon.Entities;
using SunCommon.Entities.Item;

namespace SunCommon
{
    public static class PacketStructs
    {
        public class ChannelInfo
        {
            private string name;
            private int channelNr;
            private int belongToServerNr;
            public ChannelInfo(string name, int channelNr, int belongToServerNr)
            {
                this.name = name;
                this.channelNr = channelNr;
                this.belongToServerNr = belongToServerNr;
            }

            public byte[] getBytes()
            {
                var nameBytes = ByteUtils.ToByteArray(name, 32);
                List<byte> packet = new List<byte>();
                packet.AddRange(nameBytes);
                packet.Add(00); // unk
                packet.Add((byte)belongToServerNr);
                packet.Add((byte)channelNr);
                packet.Add(01); //unk cant be 00
                packet.Add(00); //separator always 00
                return packet.ToArray();
            }
        }

        public class ServerInfo
        {
            private string name;
            private int serverNr;

            public ServerInfo(string name, int serverNr)
            {
                this.name = name;
                this.serverNr = serverNr;
            }

            public byte[] getBytes()
            {
                var nameBytes = ByteUtils.ToByteArray(name, 32);
                List<byte> packet = new List<byte>();
                packet.AddRange(nameBytes);
                packet.Add(00);
                packet.Add((byte)serverNr);
                packet.Add(00);
                packet.Add(00);
                return packet.ToArray();
            }
        }


        public class SunVector
        {
            public Single x;
            public Single y;
            public Single z;

            public SunVector(Single x, Single y, Single z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public SunVector(byte[] bytes)
            {
                this.x = BitConverter.ToSingle(new byte[] { bytes[0], bytes[1], bytes[2],bytes[3] }, 0);
                this.y = BitConverter.ToSingle(new byte[] { bytes[4], bytes[5], bytes[6], bytes[7] }, 0);
                this.z = BitConverter.ToSingle(new byte[] { bytes[8], bytes[9], bytes[10], bytes[11] }, 0);
            }

            public byte[] GetBytes()
            {
                var result = new byte[12];
                Buffer.BlockCopy(BitConverter.GetBytes(x), 0, result, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(y), 0, result, 4, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(z), 0, result, 8, 4);
                return result;
            }
        }
        public class CharacterInfo
        {
            private readonly byte slot;
            private readonly byte size;
            private readonly byte[] charName;
            private readonly byte heightCode;
            private readonly byte faceCode;
            private readonly byte hairCode;
            private readonly byte classCode;
            private readonly byte[] level;
            private readonly byte[] region;

            //private readonly SunVector position;
            private readonly byte[] posX;
            private readonly byte[] posY;
            private readonly byte[] posZ;
            private readonly byte equipNumber;
            private readonly byte[] equipInfo;
            private readonly byte unk1;
            private readonly byte[] unk2;
            private readonly byte[] unk3;
            private readonly byte[] unk4;

            public CharacterInfo(Character character)
            {
                slot = (byte)character.Slot;
                size = 16;
                charName = ByteUtils.ToByteArray(character.CharName,16);
                heightCode = (byte)character.HeightCode;
                faceCode = (byte)character.FaceCode;
                hairCode = (byte)character.HairCode;
                classCode = (byte) character.ClassCode;
                level = ByteUtils.ToByteArray(character.Level, 2);
                region = ByteUtils.ToByteArray(character.CharacterPosition.Region, 4);
                //position = new SunVector(character.CharacterPosition.LocationX,character.CharacterPosition.LocationY,character.CharacterPosition.LocationZ);
                posX = ByteUtils.ToByteArray((short)character.CharacterPosition.LocationX, 2);
                posY = ByteUtils.ToByteArray((short)character.CharacterPosition.LocationY, 2);
                posZ = ByteUtils.ToByteArray((short)character.CharacterPosition.LocationZ, 2);
                equipNumber = 0;
                equipInfo = new EquipInfo(character.Inventory.EquipItem).ToBytes();
                unk1 = 1;
                unk2 = ByteUtils.ToByteArray("", 32);
                unk3 = ByteUtils.ToByteArray("", 3);
                unk4 = ByteUtils.ToByteArray("", 4);
            }

            public CharacterInfo(int slot, int size, string charName, byte heightCode, byte faceCode, byte hairCode,
                byte classCode, byte[] level, byte[] region, byte[] posX, byte[] posY,
                byte[] posZ, byte iEquipNumber, byte[] equipInfo)
            {
                this.slot = (byte)slot;
                this.size = 0x10;
                this.charName = ByteUtils.ToByteArray(charName, 16);
                this.heightCode = heightCode;
                this.faceCode = faceCode;
                this.hairCode = hairCode;
                this.classCode = classCode;
                this.level = level;
                this.region = region;
                this.posX = posX;
                this.posY = posY;
                this.posZ = posZ;
                //this.position = new SunVector(BitConverter.ToInt16(posX,0), BitConverter.ToInt16(posY, 0), BitConverter.ToInt16(posZ, 0));
                this.equipNumber = 0;
                this.equipInfo = equipInfo;
                unk1 = 0;
                unk2 = ByteUtils.ToByteArray("", 32);
                unk3 = ByteUtils.ToByteArray("", 3);
                unk4 = ByteUtils.ToByteArray("", 4);
            }
            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.Add(slot);
                result.Add(size);
                result.AddRange(charName);
                result.Add(heightCode);
                result.Add(faceCode);
                result.Add(hairCode);
                result.Add(classCode);
                result.AddRange(level);
                result.AddRange(region);
                //result.AddRange(position.GetBytes());
                result.AddRange(posX);
                result.AddRange(posY);
                result.AddRange(posZ);
                result.Add(equipNumber);
                result.AddRange(equipInfo);
                result.Add(unk1);
                result.AddRange(unk2);
                result.AddRange(unk3);
                result.AddRange(unk4);
                return result.ToArray();
            }
        }

        public class EquipInfo
        {
            private int count;
            private List<byte> itemSlotInfos = new List<byte>();
 
            public EquipInfo(byte[] value)
            {
                count = value[0];
                for (int i = 0; i < count; i++)
                {
                    itemSlotInfos.AddRange(new ItemSlotInfo(ByteUtils.SlicedBytes(value, 1 + i * 28, 29 + i * 28)).ToBytes());
                    //itemSlotInfos.Add(new ItemSlotInfo(ByteUtils.SlicedBytes(value,1+i*28,29+i*28)));
                }
            }

            public byte[] ToBytes()
            {
                List<byte> result=new List<byte>();
                result.Add((byte)count);
                result.AddRange(itemSlotInfos);
                return result.ToArray();
            }
        }

        public class ItemSlotInfo
        {
            public int position;
            public ItemInfo itemInfo;
            public ItemOptionInfo itemOptionInfo;

            public ItemSlotInfo()
            {
                itemInfo = new ItemInfo();
                itemOptionInfo = new ItemOptionInfo();
            }
            public ItemSlotInfo(byte[] value)
            {
                position = value[0];
                itemInfo = new ItemInfo(ByteUtils.SlicedBytes(value,1,8));
                itemOptionInfo = new ItemOptionInfo(ByteUtils.SlicedBytes(value,8,value.Length));
            }

            public ItemSlotInfo(byte position, SunItem item, byte itemCount=1)
            {
                this.position = position;
                this.itemInfo = new ItemInfo(item,itemCount);
                this.itemOptionInfo = new ItemOptionInfo(item);
            }

            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.Add((byte)position);
                result.AddRange(itemInfo.ToBytes());
                result.AddRange(itemOptionInfo.ToBytes());
                return result.ToArray();
            }
        }

        public class ItemInfo
        {
            public byte[] code;
            public byte itemCount;
            public byte[] serial;

            public ItemInfo()
            {
                code=new byte[2];
                itemCount = 0;
                serial = new byte[4];
            }
            public ItemInfo(byte[] value)
            {
                code = ByteUtils.SlicedBytes(value, 0, 2);
                itemCount = value[2];
                serial = ByteUtils.SlicedBytes(value, 3, 7);
            }

            public ItemInfo(SunItem item, byte itemCount)
            {
                this.code = BitConverter.GetBytes((ushort) item.itemId);
                this.itemCount =itemCount;
                this.serial = BitConverter.GetBytes(0); //TODO figure this out
            }

            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.AddRange(code);
                result.Add(itemCount);
                result.AddRange(serial);
                return result.ToArray();
            }
        }

        public struct GmAndStateInfo
        {
            public ushort BitField1;

            public ushort GmGrade
            {
                get => BitManip.Get0to2(BitField1);
                set => BitField1 = BitManip.Set0to2(BitField1,value);
            }
            public ushort PcBangUser
            {
                get => BitManip.Get3(BitField1);
                set => BitField1 = BitManip.Set3(BitField1, value);
            }
            public ushort Condition
            {
                get => BitManip.Get4(BitField1);
                set => BitField1 = BitManip.Set4(BitField1, value);
            }
            public ushort PkState
            {
                get => BitManip.Get5to7(BitField1);
                set => BitField1 = BitManip.Set5to7(BitField1, value);
            }
            public ushort CharState
            {
                get => BitManip.Get8to15(BitField1);
                set => BitField1 = BitManip.Set5to15(BitField1, value);
            }

            public void setValue(byte[] value)
            {
                var v = BitConverter.ToUInt16(value, 0);
                GmGrade = v;
                PcBangUser = v;
                Condition = v;
                PkState = v;
                CharState = v;

            }

            public byte[] getValue()
            {
                return BitConverter.GetBytes(BitField1);
            }
        }
        public struct ItemOptionPart
        {
            public ulong BitField1;
            public ushort BitField2;
            //public unsafe fixed byte unk1[10];
            public ulong RankOption1
            {
                get => BitManip.Get0to4(BitField1);
                set => BitField1 = BitManip.Set0to4(BitField1, value);
            }
            public ulong RankOption2
            {
                get => BitManip.Get5to9(BitField1);
                set => BitField1 = BitManip.Set5to9(BitField1, value);
            }
            public ulong RankOption3
            {
                get => BitManip.Get10to14(BitField1);
                set => BitField1 = BitManip.Set10to14(BitField1, value);
            }
            public ulong RankOption4
            {
                get => BitManip.Get15to19(BitField1);
                set => BitField1 = BitManip.Set15to19(BitField1, value);
            }
            public ulong RankOption5
            {
                get => BitManip.Get20to24(BitField1);
                set => BitField1 = BitManip.Set20to24(BitField1, value);
            }
            public ulong RankOption6
            {
                get => BitManip.Get25to29(BitField1);
                set => BitField1 = BitManip.Set25to29(BitField1, value);
            }
            public ulong RankOption7
            {
                get => BitManip.Get30to34(BitField1);
                set => BitField1 = BitManip.Set30to34(BitField1, value);
            }
            public ulong RankOption8
            {
                get => BitManip.Get35to39(BitField1);
                set => BitField1 = BitManip.Set35to39(BitField1, value);
            }
            public ulong RankOption9
            {
                get => BitManip.Get40to44(BitField1);
                set => BitField1 = BitManip.Set40to44(BitField1, value);
            }
            public ulong Rank
            {
                get => BitManip.Get45to48(BitField1);
                set => BitField1 = BitManip.Set45to48(BitField1, value);
            }
            public ulong NOption
            {
                get => BitManip.Get49to51(BitField1);
                set => BitField1 = BitManip.Set49to51(BitField1, value);
            }
            public ulong Enchant
            {
                get => BitManip.Get52to55(BitField1);
                set => BitField1 = BitManip.Set52to55(BitField1, value);
            }
            public ulong SocketNumb
            {
                get => BitManip.Get56to57(BitField1);
                set => BitField1 = BitManip.Set56to57(BitField1, value);
            }
            public ulong SocketOption1
            {
                get => BitManip.Get58to63(BitField1);
                set => BitField1 = BitManip.Set58to63(BitField1, value);
            }
            public ushort SocketOption2
            {
                get => BitManip.Get0to5(BitField2);
                set => BitField2 = BitManip.Set0to5(BitField2, value);
            }
            public ushort SocketOption3
            {
                get => BitManip.Get6to11(BitField2);
                set => BitField2 = BitManip.Set6to11(BitField2, value);
            }
            public ushort Set
            {
                get => BitManip.Get12to15(BitField2);
                set => BitField2 = BitManip.Set12to15(BitField2, value);
            }

            public void setValue(byte[] value)
            {
                var v1 = BitConverter.ToUInt64(ByteUtils.SlicedBytes(value, 0, 8),0);
                var v2 = BitConverter.ToUInt16(ByteUtils.SlicedBytes(value, 9, 11),0);
                RankOption1 = v1;
                RankOption2 = v1;
                RankOption3 = v1;
                RankOption4 = v1;
                RankOption5 = v1;
                RankOption6 = v1;
                RankOption7 = v1;
                RankOption8 = v1;
                RankOption9 = v1;
                Rank = v1;
                NOption = v1;
                Enchant = v1;
                SocketNumb = v1;
                SocketOption1 = v1;
                SocketOption2 = v2;
                SocketOption3 = v2;
                Set = v2;
            }

            public byte[] getValue()
            {
                var result = new byte[10];
                Buffer.BlockCopy(BitConverter.GetBytes(BitField1),0,result,0,8);
                Buffer.BlockCopy(BitConverter.GetBytes(BitField2),0,result,8,2);
                return result;
            }


        }

        public class ItemOptionInfo
        {

            public byte[] unk1 = new byte[10];
            //pretty sure its the values like enchantment and stuff
            //private byte[] bytes8;
            //private byte[] bytes2;
            public ItemOptionPart optionPart;
            public ItemOptionInfo() { }
            public ItemOptionInfo(byte[] value)
            {
                //bytes8= ByteUtils.SlicedBytes(value, 0, 8);
                //bytes2 = ByteUtils.SlicedBytes(value, 8, 11);
                optionPart.setValue(value);
            }

            public ItemOptionInfo(SunItem item)
            {
                byte socketnumb = (byte)item.SocketNum; //TODO make socketnum to byte
                byte set = (byte)item.SetOptionType;    //TODO make setOptionType to byte
                //TODO add Socket Options
                //TODO add Rank Options
                this.optionPart.SocketNumb = socketnumb;
                this.optionPart.Set = set;

            }

            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.AddRange(optionPart.getValue());
                //result.AddRange(bytes8);
                //result.AddRange(bytes2);
                result.AddRange(unk1);
                return result.ToArray();
            }
        }

        public class StateSlotInfo
        {
            private short slotCode;
            private int time;

            public StateSlotInfo(byte[] value)
            {
                slotCode = BitConverter.ToInt16(ByteUtils.SlicedBytes(value, 0, 2),0);
                time = BitConverter.ToInt32(ByteUtils.SlicedBytes(value, 2, 6), 0);
            }

            public byte[] ToBytes()
            {
                var result = new List<byte>();
                result.AddRange(BitConverter.GetBytes(slotCode));
                result.AddRange(BitConverter.GetBytes(time));
                return result.ToArray();
            }
        }

        public class InventoryTotalInfo
        {
            public byte InvSize;
            public byte tmpInvSize;

            public ItemSlotInfo[] invslots;
            public ItemSlotInfo[] tmpInvSlots;
            public InventoryTotalInfo(ItemSlotInfo[] Invslots, ItemSlotInfo[] tempInvSlots)
            {

                int invCount=0;
                for (int i = 0; i < Invslots.Length; i++)
                {
                    if (Invslots[i] != null) invCount++;
                }

                InvSize = (byte) invCount;
                this.invslots = Invslots;

                int tmpInvCount = 0;
                for(int i = 0; i < tempInvSlots.Length; i++)
                {
                    if (tempInvSlots[i] != null) tmpInvCount++;
                }

                tmpInvSize = (byte) tmpInvCount;
                this.tmpInvSlots = tempInvSlots;
            }

            public byte[] ToBytes()
            {
                var result = new List<byte>();
                result.AddRange(BitConverter.GetBytes((short)InvSize));

                foreach (var slot in invslots)
                {
                    if(slot!=null) result.AddRange(slot.ToBytes());
                }

                foreach (var slot in tmpInvSlots)
                {
                    if (slot != null) result.AddRange(slot.ToBytes());
                }
                return result.ToArray();
            }
        }
    }
}
