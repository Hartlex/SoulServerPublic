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

        }
        private static void InitializeProtocols()
        {
            InitConnectionPackets();
            InitCharacterPackets();
        }

        private static void InitConnectionPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentConnection, out var connectionActions)) return;
            connectionActions.Add(118,OnC2SAskEnterCharSelect);
        }


        private static void InitCharacterPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentCharacter, out var characterActions)) return;
            characterActions.Add(111,OnC2SAskCreateCharacter);
        }
        private static void OnC2SAskEnterCharSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ConnectionPackets.C2SAskEnterCharSelect(buffer, connection);
            var userID = incPacket.userID;
            var cc = new ClientConnection(connection.ConnectionInfo.NetworkIdentifier,connection,new sbyte[]{00,00,00,00});
            cc.AgentConnection = connection;
            cc.UserID = userID;
            CCM.AddCC(cc);
            //TODO get charlist fromDB
            DBConnection.connection.SendObject("AskForCharacterList",userID);
            //var packet = new ConnectionPackets.S2CAnsEnterCharSelect(userID);
            //packet.Send(connection);
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
    }
}
