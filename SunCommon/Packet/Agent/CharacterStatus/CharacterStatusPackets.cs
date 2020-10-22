using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon.Packet.Agent.CharacterStatus
{
    public static class CharacterStatusPackets
    {
        public class C2SAskIncreaseAttribute : CharacterStatusPacket
        {
            public byte attribute;
            public C2SAskIncreaseAttribute(ByteBuffer buffer) : base(60)
            {
                this.attribute = buffer.ReadByte();
            }
        }

        public class S2CAnsIncreaseAttribute : CharacterStatusPacket
        {
            private byte[] objKey;
            private byte[] attribute;
            private byte[] newValue;
            public S2CAnsIncreaseAttribute(int objKey, byte attribute, int resultValue) : base(125)
            {
                this.objKey = BitConverter.GetBytes(objKey);
                this.attribute = new []{attribute};
                this.newValue = BitConverter.GetBytes(resultValue);
            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(objKey, attribute, newValue);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
