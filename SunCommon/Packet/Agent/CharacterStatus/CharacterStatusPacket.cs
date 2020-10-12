using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.CharacterStatus
{
    public class CharacterStatusPacket : SunPacket
    {
        public CharacterStatusPacket(int protocol, int size) : base(89, protocol, size) { }

        public CharacterStatusPacket(int protocol) : base(89, protocol) { }
        public CharacterStatusPacket(int protocol, Connection connection) : base(89, protocol, connection) { }
    }
}
