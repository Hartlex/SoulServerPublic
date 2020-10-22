using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Packet.Agent.Battle;

namespace MasterServer.Network.Packets.Processors
{
    internal static class BattleProcessors
    {
        internal static void OnC2SAskPlayerAttack(ByteBuffer buffer, Connection connection)
        {

            var incPacket = new BattlePackets.C2SAskPlayerAttack(buffer);
            var client = ClientManager.GetClient(connection);
            var outPacket= new BattlePackets.S2CAnsPlayerAttack(
                (uint)client.UserId,
                0,
                0,
                0,
                incPacket.targetPosition,
                incPacket.objKey,
                0,0,0,0

                );
            outPacket.Send(connection);
        }
    }
}
