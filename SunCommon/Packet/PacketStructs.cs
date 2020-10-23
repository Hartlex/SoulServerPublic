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
            public byte[] ToBytesSmall()
            {
                List<byte> result = new List<byte>();
                result.Add((byte)position);
                result.AddRange(itemInfo.ToBytes());
                result.AddRange(itemOptionInfo.ToBytes());
                return ByteUtils.SlicedBytes(result.ToArray(), 0, 5);
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
                //serial = ByteUtils.SlicedBytes(value, 3, 7);
                serial = new byte[]{0,0,0,0};
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
                RankOption1 = BitManip.Get0to4(v1);
                RankOption2 = BitManip.Get5to9(v1);
                RankOption3 = BitManip.Get10to14(v1);
                RankOption4 = BitManip.Get15to19(v1); 
                RankOption5 = BitManip.Get20to24(v1); 
                RankOption6 = BitManip.Get25to29(v1); 
                RankOption7 = BitManip.Get30to34(v1); 
                RankOption8 = BitManip.Get35to39(v1);
                RankOption9 = BitManip.Get40to44(v1);
                Rank = BitManip.Get45to48(v1);
                NOption = BitManip.Get49to51(v1);
                Enchant = BitManip.Get52to55(v1);
                SocketNumb = BitManip.Get56to57(v1);
                SocketOption1 = BitManip.Get58to63(v1);
                SocketOption2 = BitManip.Get0to5(v2);
                SocketOption3 = BitManip.Get6to11(v2);
                Set = BitManip.Get12to15(v2);
            }

            public byte[] getValue()
            {
                var result = new byte[10];
                Buffer.BlockCopy(BitConverter.GetBytes(BitField1),0,result,0,8);
                Buffer.BlockCopy(BitConverter.GetBytes(BitField2),0,result,8,2);
                return result;
            }


        }

        public struct ItemInfoX
        {
            public ushort itemId;
            public ulong bitField1;
            public ulong bitField2;
            public uint bitField3;

            public ItemInfoX(ushort itemId)
            {
                this.itemId = itemId;
                bitField1 = 0;
                bitField2 = 0;
                bitField3 = 0;
            }
            #region singleFieldDefinition
            public ulong durAmount
            {
                get => BitManip.Get0to7(bitField1);
                set => bitField1= BitManip.Set0to7(bitField1, value);
            }
            public ulong Serial
            {
                get => BitManip.Get8to15(bitField1);
                set => bitField1 = BitManip.Set8to15(bitField1, value);
            }
            public ulong Enabled
            {
                get => BitManip.Get16(bitField1);
                set => bitField1 = BitManip.Set16(bitField1, value);
            }
            public ulong unk1
            {
                get => BitManip.Get17to21(bitField1);
                set => bitField1 = BitManip.Set17to21(bitField1, value);
            }
            public ulong BaseStatIncrease
            {
                get => BitManip.Get22to24(bitField1);
                set => bitField1 = BitManip.Set22to24(bitField1, value);
            }
            public ulong Rank
            {
                get => BitManip.Get25to28(bitField1);
                set => bitField1 = BitManip.Set25to28(bitField1, value);
            }
            public ulong RankD
            {
                get => BitManip.Get29to35(bitField1);
                set => bitField1 = BitManip.Set29to35(bitField1, value);
            }
            public ulong RankC
            {
                get => BitManip.Get36to42(bitField1);
                set => bitField1 = BitManip.Set36to42(bitField1, value);
            }
            public ulong RankB
            {
                get => BitManip.Get43to49(bitField1);
                set => bitField1 = BitManip.Set43to49(bitField1, value);
            }
            public ulong RankAminus
            {
                get => BitManip.Get50to56(bitField1);
                set => bitField1 = BitManip.Set50to56(bitField1, value);
            }
            public ulong RankA
            {
                get => BitManip.Get57to63(bitField1);
                set => bitField1 = BitManip.Set57to63(bitField1, value);
            }
            public ulong RankAplus
            {
                get => BitManip.Get0to6(bitField2);
                set => bitField2 = BitManip.Set0to6(bitField2, value);
            }
            public ulong RankSminus
            {
                get => BitManip.Get7to13(bitField2);
                set => bitField2 = BitManip.Set7to13(bitField2, value);
            }
            public ulong RankS
            {
                get => BitManip.Get14to20(bitField2);
                set => bitField2 = BitManip.Set14to20(bitField2, value);
            }
            public ulong RankSplus
            {
                get => BitManip.Get21to27(bitField2);
                set => bitField2 = BitManip.Set21to27(bitField2, value);
            }
            public ulong Enchant
            {
                get => BitManip.Get28to31(bitField2);
                set => bitField2 = BitManip.Set28to31(bitField2, value);
            }
            public ulong Devine
            {
                get => BitManip.Get32(bitField2);
                set => bitField2 = BitManip.Set32(bitField2, value);
            }
            public ulong socketCount
            {
                get => BitManip.Get33to34(bitField2);
                set => bitField2 = BitManip.Set33to34(bitField2, value);
            }
            public ulong socket1
            {
                get => BitManip.Get35to42(bitField2);
                set => bitField2 = BitManip.Set35to42(bitField2, value);
            }
            public ulong socket2
            {
                get => BitManip.Get43to50(bitField2);
                set => bitField2 = BitManip.Set43to50(bitField2, value);
            }
            public ulong socket3
            {
                get => BitManip.Get51to58(bitField2);
                set => bitField2 = BitManip.Set51to58(bitField2, value);
            }
            public ulong etherDischarger
            {
                get => BitManip.Get59(bitField2);
                set => bitField2 = BitManip.Set59(bitField2, value);
            }
            public ulong unk2
            {
                get => BitManip.Get60to63(bitField2);
                set => bitField2 = BitManip.Set60to63(bitField2, value);
            }
            public uint itemstateUsed
            {
                get => BitManip.Get0(bitField3);
                set => bitField3 = BitManip.Set0(bitField3, value);
            }
            public uint itemState
            {
                get => BitManip.Get1(bitField3);
                set => bitField3 =BitManip.Set1(bitField3, value);
            }
            public uint skinId
            {
                get => BitManip.Get2to23(bitField3);
                set => bitField3 = BitManip.Set2to23(bitField3, value);
            }
            public uint unk3
            {
                get => BitManip.Get24to31(bitField3);
                set => bitField3 = BitManip.Set24to31(bitField3, value);
            }
            #endregion


            public byte[] GetBytes()
        {
            var result =new List<byte>();
            result.AddRange(BitConverter.GetBytes(itemId));
            result.AddRange(BitConverter.GetBytes(bitField1));
            result.AddRange(BitConverter.GetBytes(bitField2));
            result.AddRange(BitConverter.GetBytes(bitField3));
            return result.ToArray();
        }

        public void setValue(byte[] bytes)
        {
            //var b1 = ByteUtils.SlicedBytes(bytes, 0, 8);
            //var b2 = ByteUtils.SlicedBytes(bytes, 8, 16);
            //var b3 = ByteUtils.SlicedBytes(bytes, 16, 20);
            itemId = BitConverter.ToUInt16(bytes,0);
            bitField1 = BitConverter.ToUInt64(bytes, 2);
            bitField2 = BitConverter.ToUInt64(bytes, 10);
            bitField3 = BitConverter.ToUInt32(bytes, 18);
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


                result.Add((byte)(InvSize + tmpInvSize));


                //byte b1 = 0;
                //b1 = BitManip.Set0(b1, 0);  //1
                //b1 = BitManip.Set1(b1, 1);  
                //b1 = BitManip.Set2(b1, 0);  
                //b1 = BitManip.Set3(b1, 1);
                //b1 = BitManip.Set4(b1, 0);  //2
                //b1 = BitManip.Set5(b1, 0);
                //b1 = BitManip.Set6(b1, 0);
                //b1 = BitManip.Set7(b1, 0); 

                //byte b2 = 0;
                //b2 = BitManip.Set0(b2, 0); //3 red or not red
                //b2 = BitManip.Set1(b2, 1);
                //b2 = BitManip.Set2(b2, 0);
                //b2 = BitManip.Set3(b2, 0);
                //b2 = BitManip.Set4(b2, 0); //4
                //b2 = BitManip.Set5(b2, 0);
                //b2 = BitManip.Set6(b2, 1); //base stat increase
                //b2 = BitManip.Set7(b2, 1); //base stat increase

                //byte b3 = 0;
                //b3 = BitManip.Set0(b3, 0); //base stat increase
                //b3 = BitManip.Set1(b3, 1); //rank
                //b3 = BitManip.Set2(b3, 0); //rank
                //b3 = BitManip.Set3(b3, 0); //rank
                //b3 = BitManip.Set4(b3, 1); //rank
                //b3 = BitManip.Set5(b3, 1); // rank d
                //b3 = BitManip.Set6(b3, 0); // rank d
                //b3 = BitManip.Set7(b3, 1); // rank d

                //byte b4 = 0;
                //b4 = BitManip.Set0(b4, 0); // rank d
                //b4 = BitManip.Set1(b4, 0); // rank d
                //b4 = BitManip.Set2(b4, 0);  //rank d
                //b4 = BitManip.Set3(b4, 0);  //rank d
                //b4 = BitManip.Set4(b4, 0); //rank c
                //b4 = BitManip.Set5(b4, 0);//rank c
                //b4 = BitManip.Set6(b4, 0);//rank c
                //b4 = BitManip.Set7(b4, 1);//rank c

                //byte b5 = 0;
                //b5 = BitManip.Set0(b5, 0);//rank c
                //b5 = BitManip.Set1(b5, 0);//rank c
                //b5 = BitManip.Set2(b5, 0);//rank c
                //b5 = BitManip.Set3(b5, 1);//rank b  
                //b5 = BitManip.Set4(b5, 0);//rank b
                //b5 = BitManip.Set5(b5, 1);//rank b
                //b5 = BitManip.Set6(b5, 0);//rank b
                //b5 = BitManip.Set7(b5, 0);//rank b

                //byte b6 = 0;
                //b6 = BitManip.Set0(b6, 0);//rank b
                //b6 = BitManip.Set1(b6, 0);//rank b
                //b6 = BitManip.Set2(b6, 0);//rank a-
                //b6 = BitManip.Set3(b6, 0);//rank a-
                //b6 = BitManip.Set4(b6, 0);//rank a-
                //b6 = BitManip.Set5(b6, 1);//rank a-
                //b6 = BitManip.Set6(b6, 0);//rank a-
                //b6 = BitManip.Set7(b6, 0);//rank a-

                //byte b7 = 0;
                //b7 = BitManip.Set0(b7, 0);//rank a-
                //b7 = BitManip.Set1(b7, 1);//rank a
                //b7 = BitManip.Set2(b7, 0);//rank a
                //b7 = BitManip.Set3(b7, 1);//rank a
                //b7 = BitManip.Set4(b7, 0);//rank a
                //b7 = BitManip.Set5(b7, 0);//rank a
                //b7 = BitManip.Set6(b7, 0);//rank a
                //b7 = BitManip.Set7(b7, 0);//rank a

                //byte b8 = 0;
                //b8 = BitManip.Set0(b8, 0);//rank a+
                //b8 = BitManip.Set1(b8, 0);//rank a+
                //b8 = BitManip.Set2(b8, 0);//rank a+
                //b8 = BitManip.Set3(b8, 1);//rank a+
                //b8 = BitManip.Set4(b8, 0);//rank a+
                //b8 = BitManip.Set5(b8, 0);//rank a+
                //b8 = BitManip.Set6(b8, 0);//rank a+
                //b8 = BitManip.Set7(b8, 1);//rank s-

                //byte b9 = 0;
                //b9 = BitManip.Set0(b9, 0);//rank s-
                //b9 = BitManip.Set1(b9, 1);//rank s-
                //b9 = BitManip.Set2(b9, 0);//rank s-
                //b9 = BitManip.Set3(b9, 0);//rank s-
                //b9 = BitManip.Set4(b9, 0);//rank s-
                //b9 = BitManip.Set5(b9, 0);//rank s-
                //b9 = BitManip.Set6(b9, 0);//rank s
                //b9 = BitManip.Set7(b9, 0);//rank s

                //byte b10 = 0;
                //b10 = BitManip.Set0(b10, 0);//rank s
                //b10 = BitManip.Set1(b10, 1);//rank s
                //b10 = BitManip.Set2(b10, 0);//rank s
                //b10 = BitManip.Set3(b10, 0);//rank s
                //b10 = BitManip.Set4(b10, 0);//rank s
                //b10 = BitManip.Set5(b10, 1);//rank s+
                //b10 = BitManip.Set6(b10, 0);//rank s+
                //b10 = BitManip.Set7(b10, 1);//rank s+

                //byte b11 = 0;
                //b11 = BitManip.Set0(b11, 0);//rank s+
                //b11 = BitManip.Set1(b11, 0);//rank s+
                //b11 = BitManip.Set2(b11, 0);//rank s+
                //b11 = BitManip.Set3(b11, 0);//rank s+
                //b11 = BitManip.Set4(b11, 1);//enchant
                //b11 = BitManip.Set5(b11, 1);//enchant
                //b11 = BitManip.Set6(b11, 1);//enchant
                //b11 = BitManip.Set7(b11, 1);//enchant

                //byte b12 = 0;
                //b12 = BitManip.Set0(b12, 1); //devine
                //b12 = BitManip.Set1(b12, 1); //SocketNum
                //b12 = BitManip.Set2(b12, 1); //SocketNum
                //b12 = BitManip.Set3(b12, 1); //Socket1...
                //b12 = BitManip.Set4(b12, 1);
                //b12 = BitManip.Set5(b12, 1);
                //b12 = BitManip.Set6(b12, 0);
                //b12 = BitManip.Set7(b12, 0);

                //byte b13 = 0;
                //b13 = BitManip.Set0(b13, 0); 
                //b13 = BitManip.Set1(b13, 0); 
                //b13 = BitManip.Set2(b13,0 ); //...Socket1
                //b13 = BitManip.Set3(b13,1 ); //Socket2...
                //b13 = BitManip.Set4(b13, 1);
                //b13 = BitManip.Set5(b13, 1);
                //b13 = BitManip.Set6(b13, 0);
                //b13 = BitManip.Set7(b13, 0);


                //byte b14 = 0;
                //b14 = BitManip.Set0(b14, 0);
                //b14 = BitManip.Set1(b14, 0);
                //b14 = BitManip.Set2(b14, 0); //..Socket2 
                //b14 = BitManip.Set3(b14, 1); //Socket3...
                //b14 = BitManip.Set4(b14, 1);
                //b14 = BitManip.Set5(b14, 1);
                //b14 = BitManip.Set6(b14, 0);
                //b14 = BitManip.Set7(b14, 0);

                //byte b15 = 0;
                //b15 = BitManip.Set0(b15, 0);
                //b15 = BitManip.Set1(b15, 0);
                //b15 = BitManip.Set2(b15, 0); //...Socket3
                //b15 = BitManip.Set3(b15, 1); //Ether Discharger Mounted
                //b15 = BitManip.Set4(b15, 0);
                //b15 = BitManip.Set5(b15, 0);
                //b15 = BitManip.Set6(b15, 0);
                //b15 = BitManip.Set7(b15, 0);

                //byte b16 = 0;
                //b16 = BitManip.Set0(b16, 0); //item state 1 when extracted is skin or etheria
                //b16 = BitManip.Set1(b16, 0); //item state 0-normal 1-extracted
                //b16 = BitManip.Set2(b16, 0); //skinId...
                //b16 = BitManip.Set3(b16, 1);  
                //b16 = BitManip.Set4(b16, 1);
                //b16 = BitManip.Set5(b16, 0);
                //b16 = BitManip.Set6(b16, 1);
                //b16 = BitManip.Set7(b16, 0);

                //byte b17 = 0;
                //b17 = BitManip.Set0(b17, 0);
                //b17 = BitManip.Set1(b17, 0); 
                //b17 = BitManip.Set2(b17, 0); 
                //b17 = BitManip.Set3(b17, 0);
                //b17 = BitManip.Set4(b17, 0);
                //b17 = BitManip.Set5(b17, 0);
                //b17 = BitManip.Set6(b17, 0);
                //b17 = BitManip.Set7(b17, 0);

                //byte b18 = 0;
                //b18 = BitManip.Set0(b18, 0);
                //b18 = BitManip.Set1(b18, 0);
                //b18 = BitManip.Set2(b18, 0); 
                //b18 = BitManip.Set3(b18, 0);
                //b18 = BitManip.Set4(b18, 0);
                //b18 = BitManip.Set5(b18, 0);
                //b18 = BitManip.Set6(b18, 0);
                //b18 = BitManip.Set7(b18, 0);//...skinId

                //byte b19 = 0;
                //b19 = BitManip.Set0(b19, 1);
                //b19 = BitManip.Set1(b19, 1);
                //b19 = BitManip.Set2(b19, 1);
                //b19 = BitManip.Set3(b19, 1);
                //b19 = BitManip.Set4(b19, 1);
                //b19 = BitManip.Set5(b19, 1);
                //b19 = BitManip.Set6(b19, 1);
                //b19 = BitManip.Set7(b19, 1);
                ////byte b3 = 0;
                ////b3 = BitManip.Set0(b3, 1);
                ////b3 = BitManip.Set1to4(b3, 6); //Enchant
                ////b3 = BitManip.Set5(b3, 0);    //Devine
                ////b3 = BitManip.Set6to7(b3, 2); //SocketNum

                //result.AddRange(new byte[]
                //{

                //    100,00,
                //    5,
                //    0,36,39,1,b1,
                //    b2,b3,b4,b5,b6,
                //    b7,b8,b9,b10,b11,
                //    b12,b13,b14,b15,b16,
                //    b17,b18,0,0,0,0,0,0,0




                //    //26,00,
                //    //1,
                //    //0,36,39,1,b1,
                //    //b2,      //red or normal
                //    //b3,
                //    //b4,      //socket1
                //    //0,      //socket2
                //    //0,      //socket3
                //    //0,0,0,0,0,
                //    //0,0,0,0,0,
                //    //0,0,0,0,0

                //});
                #region test



                short size = 1;

                foreach (var slot in invslots)
                {
                    if (slot != null)
                    {
                        if (slot.ToBytes()[4] != 0)
                        {
                            //            result.AddRange(ByteUtils.SlicedBytes(slot.ToBytes(), 0, 25));
                            //            size += 25;
                            result.AddRange(slot.ToBytesSmall());
                            size += 5;
                        }
                        else
                        {
                            result.AddRange(slot.ToBytesSmall());
                            size += 5;
                        }
                    }
                }

                foreach (var slot in tmpInvSlots)
                {
                    if (slot != null)
                    {
                        result.AddRange(slot.ToBytesSmall());
                        size += 5;
                    }
                }

                result.InsertRange(0, BitConverter.GetBytes(size));
                var sb = new StringBuilder();
                foreach (var s in result)
                {
                    sb.Append(s.ToString() + "|");
                }

                var x = sb.ToString();
                #endregion

                return result.ToArray();
            }
        }

        public class MonsterRenderInfo
        {
            private byte[] objCode;
            private byte[] monsterId;
            private byte[] pos;
            private byte[] hp;
            private byte[] maxHp;
            private byte[] msRatio;
            private byte[] asRatio;
            private byte[] unk1;
            public MonsterRenderInfo(uint objCode, ushort monsterId, SunVector pos, float hp, float maxHp,
                ushort msRatio, ushort asRatio, ushort unk1)
            {
                this.objCode = BitConverter.GetBytes(objCode);
                this.monsterId = BitConverter.GetBytes(monsterId);
                this.pos = pos.GetBytes();
                this.hp = BitConverter.GetBytes(hp);
                this.maxHp = BitConverter.GetBytes(maxHp);
                this.msRatio = BitConverter.GetBytes(msRatio);
                this.asRatio = BitConverter.GetBytes(asRatio);
                this.unk1 = BitConverter.GetBytes(unk1);
            }

            public byte[] GetBytes()
            {
                var result = new List<byte>();
                result.AddRange(objCode);
                result.AddRange(monsterId);
                result.AddRange(pos);
                result.AddRange(hp);
                result.AddRange(maxHp);
                result.AddRange(msRatio);
                result.AddRange(asRatio);
                result.AddRange(unk1);
                return result.ToArray();

            }
        }

        public class UnkRenderInfo
        {
            private byte[] count;
            private byte[] unk1;

            public UnkRenderInfo(byte count)
            {
                this.count = new byte[]{count};
                this.unk1 = new byte[64];
            }

            public byte[] GetBytes()
            {
                var result = new List<byte>();
                result.AddRange(count);
                result.AddRange(unk1);
                return result.ToArray();
            }
        }

        public class SkillRenderInfo
        {
            private byte[] count;
            private byte[] skillInfo;

            public SkillRenderInfo(byte count)
            {
                this.count = new byte[]{count};
                skillInfo = new byte[96];
            }

            public byte[] GetBytes()
            {
                var result = new List<byte>();
                result.AddRange(count);
                result.AddRange(skillInfo);
                return result.ToArray();
            }
        }

    }
}
