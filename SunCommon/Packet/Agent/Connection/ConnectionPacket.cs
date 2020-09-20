using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public class ConnectionPacket :SunPacket
    {
        public ConnectionPacket(int protocol, int size) : base(72, protocol, size) { }

        public ConnectionPacket(int protocol) : base(72, protocol) { }
        public ConnectionPacket(int protocol, Connection connection) : base(72, protocol, connection) { }
    }
}
