using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using MasterServer.Database;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace MasterServer.Network.Packets.Processors
{
    internal static class SyncProcessors
    {
        private static uint objk = 0;
        internal static void OnC2sSyncMaptile(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMapTile(buffer);

        }

        internal static void OnC2SAskEnterWorld(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskEnterWorld(buffer);
            var client = ClientManager.GetClient(connection);
            client.setAtZone(atZone.atVillage);
            //var AllplayerPackage = new SyncPackets.S2CSyncAllPlayers(1);
            //AllplayerPackage.Send(connection);

            var guildInformationPacket = new SyncPackets.S2CAnsGuildInfo(client.GetSelectedCharacter());
            guildInformationPacket.Send(connection);

            var equipInformationPacket = new SyncPackets.S2CAnsEquipInfo(client.GetSelectedCharacter());
            equipInformationPacket.Send(connection);

            var positionInfoPacket = new SyncPackets.S2CAnsPlayerPositionInfo(client.GetSelectedCharacter());
            positionInfoPacket.Send(connection);
        }

        internal static void OnC2SAskKeyboardMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskKeyboardMovePacket(buffer);
            var client = ClientManager.GetClient(connection);
            client.GetSelectedCharacter().UpdatePosition(incPacket.currentPosition);
        }

        internal static void OnC2SAskMouseMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskMouseMove(buffer);
            var client = ClientManager.GetClient(connection);
            client.GetSelectedCharacter().UpdatePosition(incPacket.currentPosition);
        }

        internal static void OnC2SAskJumpMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskJumpMovePacket(buffer);
            var client = ClientManager.GetClient(connection);
            var character = client.GetSelectedCharacter();
            var pos = character.CharacterPosition;
            var posv = new PacketStructs.SunVector(pos.LocationX,pos.LocationY,pos.LocationZ);
            var outPacket = new SyncPackets.S2CItemEnter(0,objk,(uint)client.UserId,0,0,0,null,posv);
            outPacket.Send(connection);
            objk++;

            var result = new List<byte>();
            result.AddRange(new byte[]{253,174});
            //result.AddRange(BitConverter.GetBytes((ushort)32));
            result.Add(1); //count
            result.AddRange(new PacketStructs.MonsterRenderInfo(
                137,
                637,
                posv,
                100,
                100,
                1,
                1,
                0
                ).GetBytes());
            //result.Add(1);
            //result.Add(0);

            //result.Add(1);
            //result.Add(0);
            result.InsertRange(0,BitConverter.GetBytes((ushort)result.Count));
            connection.SendUnmanagedBytes(result.ToArray());
            DatabaseFunctions.UpdateFullCharacter(character);

        }

        internal static void OnC2SSyncMoveStop(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMoveStop(buffer);
            var client = ClientManager.GetClient(connection);
            client.GetSelectedCharacter().UpdatePosition(incPacket.pos);
        }


        internal static void OnC2SSyncNewPosition(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncNewPositionAfterJump(buffer);
            var client = ClientManager.GetClient(connection);
            client.GetSelectedCharacter().UpdatePosition(incPacket.pos);
        }
    }
}
