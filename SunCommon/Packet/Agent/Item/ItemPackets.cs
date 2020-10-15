using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Item
{
    public static class ItemPackets
    {
        public class C2SAskBuyItem : ItemPacket
        {
            public int unkId1; //shop iD
            public int unkId2; //npc id same as shop id
            public byte shopPage;
            public short itemIndex;
            public C2SAskBuyItem(ByteBuffer buffer) : base(149)
            {
                unkId1 = buffer.ReadInt32();
                unkId2 = buffer.ReadInt32();
                shopPage = buffer.ReadByte();
                itemIndex = buffer.ReadInt16();
            }
        }

        public class S2CAnsBuyItem : ItemPacket
        {
            private byte[] money;
            private byte[] invItemCount;
            private byte[] invSlotInfo;
            public S2CAnsBuyItem(ulong money, int inventoryItemCount, PacketStructs.ItemSlotInfo[] slots) : base(143)
            {
                this.money = BitConverter.GetBytes(money);
                this.invItemCount = BitConverter.GetBytes((short) inventoryItemCount);
                var info = new List<byte>();
                for (int i = 0; i < 75; i++)
                {
                    if(slots[i]!=null)
                        info.AddRange(slots[i].ToBytes());
                    
                }

                invSlotInfo = info.ToArray();
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(money, invItemCount, invSlotInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

        public class C2SAskDeleteItem : ItemPacket
        {
            public byte index;
            public C2SAskDeleteItem(ByteBuffer buffer) : base(187)
            {
                this.index = buffer.ReadByte();
            }
        }

        public class C2SAskUseItem : ItemPacket
        {
            public byte unk1;
            public byte index;
            public C2SAskUseItem(ByteBuffer buffer) : base(35)
            {
                this.unk1 = buffer.ReadByte();
                this.index = buffer.ReadByte();
            }
        }

        public class C2SAskEquipItem : ItemPacket
        {
            public byte unk1;
            public byte unk2;
            public byte index;
            public byte[] unk3;

            public C2SAskEquipItem(ByteBuffer buffer) : base(211)
            {
                unk1 = buffer.ReadByte();
                unk2 = buffer.ReadByte();
                index = buffer.ReadByte();
                unk3 = buffer.ReadBlock(2);
            }

        }

        public class C2SAskItemToQuickSlot : ItemPacket
        {
            public byte invIndex;
            public byte quickIndex;
            public C2SAskItemToQuickSlot(ByteBuffer buffer) : base(59)
            {
                invIndex = buffer.ReadByte();
                quickIndex = buffer.ReadByte();
            }
        }

        public class C2SAskItemMove : ItemPacket
        {
            public byte slotIdFrom;
            public byte slotIdTo;
            public byte positionFrom;
            public byte positionTo;
            public byte unk;

            public C2SAskItemMove(ByteBuffer buffer) : base(211)
            {
                slotIdFrom = buffer.ReadByte();
                slotIdTo = buffer.ReadByte();
                positionFrom = buffer.ReadByte();
                positionTo = buffer.ReadByte();
                unk = buffer.ReadByte();
            }
        }

        public class S2CAnsItemMove : ItemPacket
        {
            private byte[] ans;
            public S2CAnsItemMove(byte slotIdFrom, byte slotIdTo, byte posFrom, byte posTo, byte unk) : base(5)
            {
                ans= new byte[]
                {
                    slotIdFrom,
                    slotIdTo,
                    posFrom,
                    posTo,
                    unk
                };
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(ans);
                connection.SendUnmanagedBytes(sb);
            }
        }
        

    }
}
