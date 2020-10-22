using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon.Packet.Agent.CharacterStatus;

namespace MasterServer.Network.Packets.Processors
{
    internal static class CharacterStatusProcessors
    {
        internal static void OnC2SAskAttributeIncrease(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterStatusPackets.C2SAskIncreaseAttribute(buffer);
            var client = ClientManager.GetClient(connection);
            var character = client.GetSelectedCharacter();
            client.GetSelectedCharacter().Strength++;

            var outPacket = new CharacterStatusPackets.S2CAnsIncreaseAttribute(
                client.UserId,
                incPacket.attribute,
                1000
            );
            outPacket.Send(connection);
        }
    }
}
