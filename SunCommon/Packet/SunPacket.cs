using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public class SunPacket
    {
        public int Size { get; set; }
        public int Category { get; set; }
        public int Protocol { get; set; }
        protected Connection Connection { get; set; }

        public SunPacket(int category, int protocol, Connection connection)
        {
            this.Category = category;
            this.Protocol = protocol;
            this.Connection = connection;
        }
        public SunPacket(int category, int protocol)
        {
            this.Category = category;
            this.Protocol = protocol;
        }

        public SunPacket(int category, int protocol, int size)
        {
            this.Category = category;
            this.Protocol = protocol;
            this.Size = size;
        }
        public SunPacket(){}

        public virtual void OnReceive()
        {
            //TODO for logging purpose 
        }

        protected virtual void Send(Connection connection)
        {
           //TODO for logging purpose 
        }

        protected virtual byte[] GetSendableBytes(params byte[][] attributes)
        {
            List<byte> sb = new List<byte>();
            sb.Add((byte)Category);
            sb.Add((byte)Protocol);
            foreach (var a in attributes)
            {
                sb.AddRange(a);
            }
            sb.InsertRange(0,ByteUtils.PacketLength(sb));
            return sb.ToArray();
        }

    }

    public enum PacketCategory
    {
        AuthPackets = 51,
        AgentConnection = 72,
        AgentCharacter = 165,
        AgentSync = 253,
        CharacterStatus = 89,
        Item =33
    }
}
