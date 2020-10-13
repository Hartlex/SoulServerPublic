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
            private byte[] tmpInvItemCount;
            private byte[] invSlotInfo;
            public S2CAnsBuyItem(long money, int inventoryItemCount,int tempInventoryItemCount, PacketStructs.ItemSlotInfo[] slots) : base(143)
            {
                this.money = BitConverter.GetBytes(money);
                this.invItemCount = new []{(byte)inventoryItemCount};
                this.tmpInvItemCount = new[] { (byte)tempInventoryItemCount };
                var info = new List<byte>();
                for (int i = 0; i < inventoryItemCount; i++)
                {
                    info.AddRange(slots[i].ToBytes());
                    
                }

                invSlotInfo = info.ToArray();
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(money, invItemCount,tmpInvItemCount, invSlotInfo);
                connection.SendUnmanagedBytes(sb);
            }
        }

    }
}
