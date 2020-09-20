using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public class AuthPacket : SunPacket
    {
        public AuthPacket(int protocol, int size) : base(51, protocol, size){}

        public AuthPacket(int protocol) : base(51,protocol){}
        public AuthPacket(int protocol, Connection connection) : base(51, protocol,connection) { }
    }
}
