using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Battle
{
    public class BattlePacket : SunPacket
    {
        public BattlePacket(int protocol, int size) : base(60, protocol, size) { }

        public BattlePacket(int protocol) : base(60, protocol) { }
        public BattlePacket(int protocol, Connection connection) : base(60, protocol, connection) { }
    }
}
