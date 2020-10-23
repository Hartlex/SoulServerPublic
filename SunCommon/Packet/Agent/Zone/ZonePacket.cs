using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Zone
{
    public class ZonePacket : SunPacket
    {
        public ZonePacket(int protocol, int size) : base(111, protocol, size) { }

        public ZonePacket(int protocol) : base(111, protocol) { }
        public ZonePacket(int protocol, Connection connection) : base(111, protocol, connection) { }
    }
}
