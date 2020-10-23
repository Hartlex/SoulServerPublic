using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Inventory
{
    public class InventoryPacket : SunPacket
    {
        public InventoryPacket(int protocol, int size) : base(101, protocol, size) { }

        public InventoryPacket(int protocol) : base(101, protocol) { }
        public InventoryPacket(int protocol, Connection connection) : base(101, protocol, connection) { }
    }
}
