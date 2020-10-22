using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Entities.Map;
using SunCommon.Packet.Agent.Zone;
using SunCommon.Parser;

namespace MasterServer.Network.Packets.Processors
{
    internal static class ZoneProcessors
    {
        internal static void OnC2SAskMapMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ZonePackets.C2SAskMoveMap(buffer);
            var x = ByteUtils.SlicedBytes(buffer.Data, 4, 12);
            var sb = new StringBuilder();
            foreach(var y in x)
            {
                sb.Append(y + "|");
            }

            var z = sb.ToString();

            var cpos = ClientManager.GetClient(connection).GetSelectedCharacter().CharacterPosition;
            var portal = PortalManager.getPortal(incPacket.key1, incPacket.key2);
            var newPos = MapManager.GetMap(portal.fieldTo).wayPointPosition;
            cpos.Region = portal.fieldTo;
            cpos.LocationX = newPos.x;
            cpos.LocationY = newPos.y;
            cpos.LocationZ = newPos.z;
            var outPacket = new ZonePackets.S2CAnsMoveMap(portal.id);
            outPacket.Send(connection);
        }
    }
}
