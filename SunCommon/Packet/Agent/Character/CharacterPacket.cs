using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public class CharacterPacket : SunPacket
    {
        public CharacterPacket(int protocol, int size) : base(165, protocol, size) { }

        public CharacterPacket(int protocol) : base(165, protocol) { }
        public CharacterPacket(int protocol, Connection connection) : base(165, protocol, connection) { }
    }
}
