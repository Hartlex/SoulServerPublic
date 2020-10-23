using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Console.WriteLine(incPacket.attackType+"  "+ incPacket.styleCode);
            var outPacket= new BattlePackets.S2CAnsPlayerAttack(
                (uint)client.UserId,
                incPacket.attackType,
                incPacket.styleCode,
                0,
                incPacket.targetPosition,
                incPacket.objKey,
                0,0,0,5

                );
            for(int i = 100; i < 255;i++)
            {
                var result = new byte[]
                {
                    03, 00,
                    60, (byte)i,
                    2
                };
                Console.WriteLine("Testing:"+i);
                connection.SendUnmanagedBytes(result);
                Thread.Sleep(1000);
            }

            //outPacket.Send(connection);
        }
    }
}
