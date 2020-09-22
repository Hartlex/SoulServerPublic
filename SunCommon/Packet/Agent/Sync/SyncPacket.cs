using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public class SyncPacket : SunPacket
    {
        public SyncPacket(int protocol, int size) : base(253, protocol, size) { }

        public SyncPacket(int protocol) : base(253, protocol) { }
        public SyncPacket(int protocol, Connection connection) : base(253, protocol, connection) { }
    }
}
