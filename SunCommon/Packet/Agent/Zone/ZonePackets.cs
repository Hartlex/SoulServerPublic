using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.Zone
{
    public static class ZonePackets
    {
        public class C2SAskMoveMap : ZonePacket
        {
            public byte unk1;
            public byte key1;
            public byte key2;
            public C2SAskMoveMap(ByteBuffer buffer) : base(204)
            {
                unk1 = buffer.ReadByte();
                key1 = buffer.ReadByte();
                key2 = buffer.ReadByte();
            }
        }

        public class S2CAnsMoveMap : ZonePacket
        {
            private byte[] portalId;

            public S2CAnsMoveMap(ulong portalId) : base(108)
            {
                this.portalId = BitConverter.GetBytes(portalId);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(portalId);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
