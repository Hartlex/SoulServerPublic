using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Item
{
    public class ItemPacket : SunPacket { 
        public ItemPacket(int protocol, int size) : base(33, protocol, size) { }

        public ItemPacket(int protocol) : base(33, protocol) { }
        public ItemPacket(int protocol, Connection connection) : base(33, protocol, connection) { }
    }
}
