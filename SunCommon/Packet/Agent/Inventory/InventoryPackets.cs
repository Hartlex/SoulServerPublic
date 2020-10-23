using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Packet.Agent.Inventory
{
    public static class InventoryPackets
    {
        public class C2SOpenInventory : InventoryPacket
        {
            public C2SOpenInventory() : base(68)
            {

            }
        }
    }
}
