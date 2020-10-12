using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;

namespace SunCommon.Packet.Agent.Item
{
    public static class ItemPackets
    {
        public class OnC2SAskBuyItem : ItemPacket
        {
            public int unkId1; //shop iD
            public int unkId2; //npc id same as shop id
            public byte shopPage;
            public short itemIndex;
            public OnC2SAskBuyItem(ByteBuffer buffer) : base(149)
            {
                unkId1 = buffer.ReadInt32();
                unkId2 = buffer.ReadInt32();
                shopPage = buffer.ReadByte();
                itemIndex = buffer.ReadInt16();
            }
        }
    }
}
