using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using KaymakNetwork;
using MasterServer.Clients;
using MasterServer.Database;
using MasterServer.Properties;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Entities;
using SunCommon.Packet.Agent.Character;
using SunCommon.Packet.Agent.CharacterStatus;
using SunCommon.Packet.Agent.Item;

namespace MasterServer.Network.Packets
{
    internal static class PacketProcessor   
    {
        public static Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>> AllPackets = new Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>>();
        public static bool FindPacketAction(PacketCategory category, int protocol, out Action<ByteBuffer, Connection> action)
        {
            action = null;
            return AllPackets.TryGetValue(category, out var Category) && Category.TryGetValue(protocol, out action);
        }
        public static void Initialize()
        {
            Console.WriteLine(Resources.PacketProcessor_Initialize_Load);
            InitializeCategories();
            InitializeProtocols();
            Console.WriteLine(Resources.PacketProcessor_Initialize_Success);
        }

        private static void InitializeCategories()
        {
            Dictionary<int, Action<ByteBuffer, Connection>> authActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AuthPackets, authActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load,PacketCategory.AuthPackets.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> connectionActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentConnection, connectionActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentConnection.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> characterActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentCharacter, characterActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentCharacter.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> syncActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentSync, syncActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentSync.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> characterStatusActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.CharacterStatus, characterStatusActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.CharacterStatus.ToString());

            Dictionary<int,Action<ByteBuffer,Connection>> itemActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.Item,itemActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.Item.ToString());
        }

        private static void InitializeProtocols()
        {
            InitAuthPackets();
            InitConnectionPackets();
            InitCharacterPackets();
            InitSyncPackets();
            InitCharacterStatusPackets();
            InitItemPackets();
        }

        private static void InitItemPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.Item, out var itemActions)) return;
            itemActions.Add(149,OnC2SAskBuyItem);
        }

        private static void OnC2SAskBuyItem(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.OnC2SAskBuyItem(buffer);
        }

        private static void InitCharacterStatusPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.CharacterStatus, out var characterStatusActions)) return;
            characterStatusActions.Add(60,OnC2SAskAttributeIncrease);
        }

        private static void OnC2SAskAttributeIncrease(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterStatusPackets.C2SAskIncreaseAttribute(buffer);
            var client = ClientManager.GetClient(connection);
            var character = client.GetSelectedCharacter();
            client.GetSelectedCharacter().Strength++;

            var outPacket = new CharacterStatusPackets.S2CAnsIncreaseAttribute(
                client.objectKey,
                incPacket.attribute,
                (uint)character.Strength
                );
            outPacket.Send(connection);
        }

        private static void InitAuthPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AuthPackets, out var authActions)) return;
            authActions.Add(1, OnC2SAskConnect);
            authActions.Add(3, OnC2SAskLogin);
            authActions.Add(15, OnC2SAskForServerList);
            authActions.Add(19, OnC2SAskServerSelect);
        }
        private static void InitConnectionPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentConnection, out var connectionActions)) return;
            connectionActions.Add(118, OnC2SAskEnterCharSelect);
            connectionActions.Add(31, OnC2SAskEnterGame);
            connectionActions.Add(223, OnC2SAskPrepareWorld);
            connectionActions.Add(40,OnC2SErrorMessage);
        }

        private static void OnC2SErrorMessage(ByteBuffer buffer, Connection connection)
        {
            //230|1|
            //72|40|
            //116|166|105|0|0|0|64|0|0|48|188|0|116|166|41|0|83|85|78|71|65|77|69|46|69|88|69|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|99|110|49|48|48|54|49|55|40|127|233|105|95|18|0|0|87|105|110|100|111|119|115|32|49|48|32|69|110|116|101|114|112|114|105|115|101|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|73|110|116|101|108|40|82|41|32|88|101|111|110|40|82|41|32|67|80|85|32|69|51|45|49|53|48|53|77|32|118|54|32|64|32|51|46|48|48|71|72|122|32|40|56|32|67|80|85|115|41|44|32|126|51|46|48|71|72|122|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|49|54|51|56|52|77|66|32|82|65|77|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|68|105|114|101|99|116|88|32|49|50|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|73|110|116|101|108|40|82|41|32|72|68|32|71|114|97|112|104|105|99|115|32|80|54|51|48|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|52|48|57|53|32|77|66|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|50|51|46|50|48|46|48|48|49|54|46|52|57|55|51|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|
            Console.WriteLine(Encoding.ASCII.GetString(buffer.Data));
        }

        private static void InitCharacterPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentCharacter, out var characterActions)) return;
            characterActions.Add(51,OnC2SAskSelectPlayer);
        }

        private static void OnC2SAskSelectPlayer(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterPackets.C2SAskSelectPlayer(buffer);
            var client = ClientManager.GetClient(connection);
            var character = client.GetSelectedCharacter();
            client.objectKey = incPacket.objectKey;
            var outPacket = new CharacterPackets.S2CAnsSelectPlayer(
                incPacket.objectKey,
                character.Hp,
                character.MaxHp,
                character.Level
            );
            outPacket.Send(connection);
        }

        private static void InitSyncPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentSync, out var syncActions)) return;
            syncActions.Add(141, OnC2SAskEnterWorld);
            syncActions.Add(115, OnC2SAskJumpMove);
            syncActions.Add(202, OnC2SAskMouseMove);
            syncActions.Add(43, OnC2SAskKeyboardMove);
            syncActions.Add(69,OnC2SSyncNewPosition);
            syncActions.Add(123, OnC2SSyncMoveStop);
            syncActions.Add(96,OnC2sSyncMaptile);
        }


        private static void OnC2SSyncMoveStop(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMoveStop(buffer);
        }


        private static void OnC2SSyncNewPosition(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncNewPositionAfterJump(buffer);
        }


        #region authPackets

        private static void OnC2SAskConnect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskConnect(buffer, connection);
            //Get Protocol Version
            var protocolBytes = incPacket.ProtocolVersion;
            var protocol = protocolBytes[0]+"."+protocolBytes[1] + "." + protocolBytes[2];
            
            var address = incPacket.serverAddress; //Could also be Client Address
            //Check if Protocol Versions Match
            var canConnect = (protocol == ConfigurationManager.AppSettings["protocolVersion"]);
            var outPacket = new AuthPackets.S2CAnsConnect(canConnect);
            outPacket.Send(connection);
        }
        private static void OnC2SAskLogin(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskLogin(buffer, connection);
            var success = DatabaseFunctions.UserLogin(incPacket.Username, incPacket.DecPassword, out var userID);
            if (success)
            {
                ClientManager.GetClient(connection).UserId = userID;
                ClientManager.GetClient(connection).setAtZone(atZone.afterLogin);
            }
            var outPacket = new AuthPackets.S2CAnsLogin(success);
            outPacket.Send(connection);
        }

        private static void OnC2SAskForServerList(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskForServerList(connection);
            // TODO implement actuall server and Channels
            var serverInfo = Server.getServerInfos();
            var channelInfo = Server.getChannelInfos();
            //var server = new PacketStructs.ServerInfo("Etherain", 1);
            //var channel = new PacketStructs.ChannelInfo("Channel 1", 1, 1);
            var serverInfoPacket = new AuthPackets.S2CServerInfo(serverInfo.ToArray());
            var channeInfoPacket = new AuthPackets.S2CChannelInfo(channelInfo.ToArray());
            serverInfoPacket.Send(connection);
            channeInfoPacket.Send(connection);
        }

        private static void OnC2SAskServerSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskServerSelect(buffer, connection);
            var client = ClientManager.GetClient(connection);
            int userID = client.UserId;
            var connectedServer = Server.getServer(incPacket.server);
            var connectedChannel = Server.getChannel(incPacket.server, incPacket.channel);

            connectedServer.ConnectToServer(client);
            connectedChannel.ConnectToChannel(client);

            string ip = "127.0.0.1";
            int port = connectedServer.port;
            //AgentConnection.connection.SendObject("UserEnterCharSelect",userID);
            var outPacket = new AuthPackets.S2CAnsServerSelect(userID, ip, port);
            outPacket.Send(connection);
        }
        #endregion

        #region connectionPackets

        private static void OnC2SAskEnterCharSelect(ByteBuffer buffer, Connection connection)
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
            }
        }

        private static void OnC2SAskEnterGame(ByteBuffer buffer, Connection connection)
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

            var packet = new ConnectionPackets.S2CAnsEnterGame(character.Id);
            packet.Send(connection);

        }
        private static void OnC2SAskPrepareWorld(ByteBuffer buffer, Connection connection)
        {
            var packet = new ConnectionPackets.C2SAskWorldPrepare();
            var client = ClientManager.GetClient(connection);

            string ip = "127.0.0.1";

            int port = client.GetChannel().worldPort;
            var outPacket = new ConnectionPackets.S2CAnsWorldPrepare(ip, port);
            outPacket.Send(connection);
        }
        #endregion

        #region SyncPackets
        private static void OnC2sSyncMaptile(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SSyncMapTile(buffer);

        }

        private static void OnC2SAskEnterWorld(ByteBuffer buffer, Connection connection)
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

        private static void OnC2SAskKeyboardMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskKeyboardMovePacket(buffer);
            var client = ClientManager.GetClient(connection);
        }

        private static void OnC2SAskMouseMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskMouseMove(buffer);
        }

        private static void OnC2SAskJumpMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new SyncPackets.C2SAskJumpMovePacket(buffer);

        }

        #endregion

    }
}