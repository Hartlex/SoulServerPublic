using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities;

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
            private short x;
            private short y;
            private short z;

            public SunVector(short x, short y, short z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public SunVector(byte[] bytes)
            {
                this.x = BitConverter.ToInt16(new byte[] { bytes[0], bytes[1] }, 0);
                this.y = BitConverter.ToInt16(new byte[] { bytes[2], bytes[3] }, 0);
                this.z = BitConverter.ToInt16(new byte[] { bytes[4], bytes[5] }, 0);
            }

            public byte[] GetBytes()
            {
                var result = new byte[6];
                Buffer.BlockCopy(BitConverter.GetBytes(x), 0, result, 0, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(y), 0, result, 2, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(z), 0, result, 4, 2);
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

            private readonly SunVector position;
            //private readonly byte[] posX;
            //private readonly byte[] posY;
            //private readonly byte[] posZ;
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
                position = new SunVector(character.CharacterPosition.LocationX,character.CharacterPosition.LocationY,character.CharacterPosition.LocationZ);
                //posX = ByteUtils.ToByteArray(character.CharacterPosition.LocationX, 2);
                //posY = ByteUtils.ToByteArray(character.CharacterPosition.LocationY, 2);
                //posZ = ByteUtils.ToByteArray(character.CharacterPosition.LocationZ, 2);
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
                this.position = new SunVector(BitConverter.ToInt16(posX,0), BitConverter.ToInt16(posY, 0), BitConverter.ToInt16(posZ, 0));
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
                result.AddRange(position.GetBytes());
                //result.AddRange(posX);
                //result.AddRange(posY);
                //result.AddRange(posZ);
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
            private int position;
            private ItemInfo itemInfo;
            private ItemOptionInfo itemOptionInfo;

            public ItemSlotInfo(byte[] value)
            {
                position = value[0];
                itemInfo = new ItemInfo(ByteUtils.SlicedBytes(value,1,8));
                itemOptionInfo = new ItemOptionInfo(ByteUtils.SlicedBytes(value,8,value.Length));
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
            private byte[] code;
            private byte durability;
            private byte[] serial;

            public ItemInfo(byte[] value)
            {
                code = ByteUtils.SlicedBytes(value, 0, 2);
                durability = value[2];
                serial = ByteUtils.SlicedBytes(value, 3, 7);
            }

            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.AddRange(code);
                result.Add(durability);
                result.AddRange(serial);
                return result.ToArray();
            }
        }

        public class ItemOptionInfo
        {
            private byte[] unk1 = new byte[]{204,204,204,204,204,204,204,204,204,204};
            //pretty sure its the values like enchantment and stuff
            private byte[] bytes8;
            private byte[] bytes2;

            public ItemOptionInfo(byte[] value)
            {
                bytes8= ByteUtils.SlicedBytes(value, 0, 8);
                bytes2 = ByteUtils.SlicedBytes(value, 8, 11);
            }

            public byte[] ToBytes()
            {
                List<byte> result = new List<byte>();
                result.AddRange(bytes8);
                result.AddRange(bytes2);
                result.AddRange(unk1);
                return result.ToArray();
            }
        }
    }
}
