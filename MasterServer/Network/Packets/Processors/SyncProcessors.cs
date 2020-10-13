using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace MasterServer.Network.Packets.Processors
{
    internal static class SyncProcessors
    {
        internal static void OnC2sSyncMaptile(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMapTile(buffer);

        }

        internal static void OnC2SAskEnterWorld(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskEnterWorld(buffer);
            var client = ClientManager.GetClient(connection);

            var positionInfoPacket = new SyncPackets.S2CAnsPlayerPositionInfo(client.GetSelectedCharacter());
            positionInfoPacket.Send(connection);

            var guildInformationPacket = new SyncPackets.S2CAnsGuildInfo(client.GetSelectedCharacter());
            guildInformationPacket.Send(connection);

            var equipInformationPacket = new SyncPackets.S2CAnsEquipInfo(client.GetSelectedCharacter());
            equipInformationPacket.Send(connection);
        }

        internal static void OnC2SAskKeyboardMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskKeyboardMovePacket(buffer);
            var client = ClientManager.GetClient(connection);
        }

        internal static void OnC2SAskMouseMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskMouseMove(buffer);
        }

        internal static void OnC2SAskJumpMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskJumpMovePacket(buffer);

        }

        internal static void OnC2SSyncMoveStop(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMoveStop(buffer);
        }


        internal static void OnC2SSyncNewPosition(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncNewPositionAfterJump(buffer);
        }
    }
}
