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
using SunCommon.Entities;
using SunCommon.Packet.Agent.Character;

namespace MasterServer.Network.Packets.Processors
{
    internal static class ConnectionProcessors
    {
        internal static void OnC2SAskEnterCharSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ConnectionPackets.C2SAskEnterCharSelect(buffer, connection);
            if (DatabaseFunctions.getAllCharacters(incPacket.userID, out var characterInfos))
            {
                int userId = ClientManager.GetClient(connection).UserId;
                var bytes = new List<byte>();
                bytes.AddRange(ByteUtils.ToByteArray(userId, 4));
                bytes.Add((byte)characterInfos.Count);
                bytes.Add((byte)characterInfos.Count);
                foreach (var info in characterInfos)
                {
                    bytes.AddRange(info.ToBytes());
                }
                var outPacket = new ConnectionPackets.S2CAnsEnterCharSelect(bytes.ToArray());
                outPacket.Send(connection);

                var outPacket2 = new ConnectionPackets.S2CAnsWorldPrepare("127.0.0.1",8010);
                outPacket2.Send(connection);

            }
        }

        internal static void OnC2SAskEnterGame(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ConnectionPackets.C2SAskEnterGame(buffer, connection);
            var client = ClientManager.GetClient(connection);
            var userId = client.UserId;
            DatabaseFunctions.GetFullCharacter(userId, incPacket.charSlot, out var fullCharBytes);
            var character = new Character(fullCharBytes);
            client.SelectCharacter(character);
            var playerInfoPacket = new CharacterPackets.S2CCharacterInfo(character);
            playerInfoPacket.Send(connection);

            var skillInfoPacket = new CharacterPackets.S2CSkillInfo(character);
            skillInfoPacket.Send(connection);

            var quickInfoPacket = new CharacterPackets.S2CQuickInfo(character);
            quickInfoPacket.Send(connection);

            var styleInfoPacket = new CharacterPackets.S2CStyleInfo(character);
            styleInfoPacket.Send(connection);

            var stateInfoPacket = new CharacterPackets.S2CStatePacket(character);
            stateInfoPacket.Send(connection);

            var packet = new ConnectionPackets.S2CAnsEnterGame(client.UserId);
            packet.Send(connection);

        }
        internal static void OnC2SAskPrepareWorld(ByteBuffer buffer, Connection connection)
        {
            var packet = new ConnectionPackets.C2SAskWorldPrepare();
            var client = ClientManager.GetClient(connection);

            string ip = "127.0.0.1";

            int port = client.GetChannel().worldPort;
            var outPacket = new ConnectionPackets.S2CAnsWorldPrepare(ip, port);
            outPacket.Send(connection);
        }
        internal static void OnC2SErrorMessage(ByteBuffer buffer, Connection connection)
        {
            //230|1|
            //72|40|
            //116|166|105|0|0|0|64|0|0|48|188|0|116|166|41|0|83|85|78|71|65|77|69|46|69|88|69|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|99|110|49|48|48|54|49|55|40|127|233|105|95|18|0|0|87|105|110|100|111|119|115|32|49|48|32|69|110|116|101|114|112|114|105|115|101|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|73|110|116|101|108|40|82|41|32|88|101|111|110|40|82|41|32|67|80|85|32|69|51|45|49|53|48|53|77|32|118|54|32|64|32|51|46|48|48|71|72|122|32|40|56|32|67|80|85|115|41|44|32|126|51|46|48|71|72|122|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|49|54|51|56|52|77|66|32|82|65|77|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|68|105|114|101|99|116|88|32|49|50|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|73|110|116|101|108|40|82|41|32|72|68|32|71|114|97|112|104|105|99|115|32|80|54|51|48|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|52|48|57|53|32|77|66|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|50|51|46|50|48|46|48|48|49|54|46|52|57|55|51|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|
            Console.WriteLine(Encoding.ASCII.GetString(buffer.Data));
        }

        internal static void S2CPrepareworldConnectCMD(ByteBuffer buffer, Connection connection)
        {
            var outPacket = new ConnectionPackets.S2CWorldPrepareCMD();
            outPacket.Send(connection);
        }

        internal static void OnC2SAskBackToCharSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ConnectionPackets.C2SAskBackToCharSelect();
            var outPacket = new ConnectionPackets.S2CAnsBackToCharSelect();
            outPacket.Send(connection);
        }
    }
}
