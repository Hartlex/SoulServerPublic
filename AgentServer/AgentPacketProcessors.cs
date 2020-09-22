using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Packet.Agent.Character;

namespace AgentServer
{
    internal static class AgentPacketProcessors
    {
        public static Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>> AllPackets = new Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>>();
        public static bool FindPacketAction(PacketCategory category, int protocol, out Action<ByteBuffer, Connection> action)
        {
            action = null;
            if (!AllPackets.TryGetValue(category, out var Category)) return false;
            return Category.TryGetValue(protocol, out action);
        }
        public static void Initialize()
        {
            InitializeCategories();
            InitializeProtocols();
        }
        private static void InitializeCategories()
        {
            Dictionary<int, Action<ByteBuffer, Connection>> connectionActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentConnection, connectionActions);

            Dictionary<int, Action<ByteBuffer, Connection>> characterActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentCharacter, characterActions);

            Dictionary<int, Action<ByteBuffer, Connection>> syncActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentSync, syncActions);

        }
        private static void InitializeProtocols()
        {
            InitConnectionPackets();
            InitCharacterPackets();
            InitSyncPackets();
        }

        private static void InitSyncPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentSync, out var syncActions)) return;
            syncActions.Add(141,OnC2SAskEnterWorld);
        }

        private static void InitConnectionPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentConnection, out var connectionActions)) return;
            connectionActions.Add(118,OnC2SAskEnterCharSelect);
            connectionActions.Add(31, OnC2SAskEnterGame);
        }


        private static void InitCharacterPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentCharacter, out var characterActions)) return;
            characterActions.Add(111,OnC2SAskCreateCharacter);
            characterActions.Add(137,OnC2SAskDeleteCharacter);
            characterActions.Add(81,OnC2SAskDuplicateName);

        }
        private static void OnC2SAskEnterCharSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ConnectionPackets.C2SAskEnterCharSelect(buffer, connection);
            var userID = incPacket.userID;
            var cc = new ClientConnection(connection.ConnectionInfo.NetworkIdentifier,connection,new sbyte[]{00,00,00,00});
            cc.AgentConnection = connection;
            cc.UserID = userID;
            CCM.AddCC(cc);
            DBConnection.connection.SendObject("AskForCharacterList", userID);
        }

        private static void OnC2SAskDuplicateName(ByteBuffer buffer, Connection connection)
        {
            var incPacket= new CharacterPackets.C2SAskDublicateNameCheck(buffer,connection);
            var userID = CCM.GetClientConnection(connection).UserID;
            DBConnection.connection.SendObject("AskDuplicateName", new[]{incPacket.name,userID.ToString()});
        }
        private static void OnC2SAskCreateCharacter(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new CharacterPackets.C2SAskCreateCharacter(buffer,connection);
            var userId = CCM.GetClientConnection(connection).UserID;
            var chaInfo = new string[]
            {
             userId.ToString(),
             incPacket.CharName,
             incPacket.ClassCode.ToString(),
             incPacket.HeightCode.ToString(),
             incPacket.FaceCode.ToString(),
             incPacket.HairCode.ToString()
            };
            
            DBConnection.connection.SendObject("CreateCharacter",chaInfo);
        }

        private static void OnC2SAskDeleteCharacter(ByteBuffer buffer, Connection connection)
        {
            var packet = new CharacterPackets.C2SAskDeleteCharacter(buffer,connection);
            var userID = CCM.GetClientConnection(connection).UserID;
            DBConnection.connection.SendObject("DeleteCharacter",new[]{userID,packet.charSlot});
        }

        private static void OnC2SAskEnterGame(ByteBuffer buffer, Connection connection)
        {
            //byte charSlot=0;
            var selectedCharSlot = buffer.ReadByte();
            short charSlot = buffer.ReadInt16();
            var x = charSlot / 128;
            var packet = new ConnectionPackets.C2SAskEnterGame((byte)x,connection);
            var userID = CCM.GetClientConnection(connection).UserID;
            DBConnection.connection.SendObject("GetFullCharacter",new[]{userID,packet.charSlot});
        }

        private static void OnC2SAskEnterWorld(ByteBuffer buffer, Connection connection)
        {
            
            var incPacket= new SyncPackets.C2SAskEnterWorld(buffer);
            var cc = CCM.GetClientConnection(connection);

            var positionInfoPacket = new SyncPackets.S2CAnsPlayerPositionInfo(cc.Character);
            positionInfoPacket.Send(connection);

            var guildInformationPacket = new SyncPackets.S2CAnsGuildInfo(cc.Character);
            guildInformationPacket.Send(connection);

            var equipInformationPacket = new SyncPackets.S2CAnsEquipInfo(cc.Character);
            equipInformationPacket.Send(connection);
        }
    }
}
