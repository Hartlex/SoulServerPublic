using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using static SunCommon.PacketStructs;

namespace SunCommon.Packet.Agent.Battle
{
    public static class BattlePackets
    {
        public class C2SAskPlayerAttack : BattlePacket
        {
            public uint clientSerial;
            public byte attackType;
            public ushort unk1;
            public ushort styleCode;
            public uint objKey;
            public SunVector currentPosition;
            public SunVector targetPosition;

            public C2SAskPlayerAttack(ByteBuffer buffer) : base(12)
            {
                this.clientSerial = buffer.ReadUInt32();
                this.attackType = buffer.ReadByte();
                this.unk1 = buffer.ReadUInt16();
                this.styleCode = buffer.ReadUInt16();
                this.objKey = buffer.ReadUInt32();
                this.currentPosition = new SunVector(buffer.ReadBlock(12));
                this.targetPosition = new SunVector(buffer.ReadBlock(12));
            }
        }

        public class S2CAnsPlayerAttack : BattlePacket
        {
            private byte[] attackerKey;
            private byte[] attackType;
            private byte[] styleCode;
            private byte[] clientSerial;
            private byte[] position;
            private byte[] targetKey;
            private byte[] damage;
            private byte[] targetHp;
            private byte[] unk;
            private byte[] effect;

            public S2CAnsPlayerAttack(uint attackerKey, byte attackType, ushort styleCode, uint clientSerial,
                SunVector position, uint targetKey, ushort damage, uint targetHp, byte unk, byte effect) :base(109)
            {
                this.attackerKey = BitConverter.GetBytes(attackerKey);
                this.attackType = BitConverter.GetBytes(attackType);
                this.styleCode = BitConverter.GetBytes(styleCode);
                this.clientSerial = BitConverter.GetBytes(clientSerial);
                this.position = position.GetBytes();
                this.targetKey = BitConverter.GetBytes(targetKey);
                this.damage = BitConverter.GetBytes(damage);
                this.targetHp = BitConverter.GetBytes(targetHp);
                this.unk = new []{unk};
                this.effect = new[] {effect};

            }

            public new void Send(Connection connection)
            {
                var sb = GetSendableBytes(attackerKey, attackType, styleCode, clientSerial, position, targetKey, damage,
                    targetHp, unk, effect);
                connection.SendUnmanagedBytes(sb);
            }
        }
    }
}
