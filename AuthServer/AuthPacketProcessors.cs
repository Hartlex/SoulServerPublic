using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace AuthServer
{
    internal static class AuthPacketProcessors
    {
        private static Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>> AllPackets = new Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>>();

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
            Dictionary<int, Action<ByteBuffer, Connection>> authActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AuthPackets,authActions);

        }
        private static void InitializeProtocols()
        {
            InitAuthPackets();
        }

        private static void InitAuthPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AuthPackets, out var authActions)) return;
            authActions.Add(1,OnC2SAskConnect);
            authActions.Add(3,OnC2SAskLogin);
            authActions.Add(15,OnC2SAskForServerList);
            authActions.Add(19,OnC2SAskServerSelect);
        }

        private static void OnC2SAskConnect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskConnect(buffer,connection);
            var protocol = incPacket.ProtocolVersion;
            var Address = incPacket.serverAddress; //Could also be Client Address
            //TODO check if he should be able to connect
            var canConnect = true;
            var outPacket = new AuthPackets.S2CAnsConnect(canConnect);
            outPacket.Send(connection);
        }

        private static void OnC2SAskLogin(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskLogin(buffer,connection);
            DBConnection.connection.SendObject("UserLogin", new[] { incPacket.Username, incPacket.DecPassword, connection.ConnectionInfo.NetworkIdentifier.ToString() });
        }

        private static void OnC2SAskForServerList(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskForServerList(connection);
            // TODO implement actuall server and Channels
            var server = new PacketStructs.ServerInfo("Etherain", 1);
            var channel = new PacketStructs.ChannelInfo("Channel 1", 1, 1);
            var serverInfoPacket = new AuthPackets.S2CServerInfo(server);
            var channeInfoPacket = new AuthPackets.S2CChannelInfo(channel);
            serverInfoPacket.Send(connection);
            channeInfoPacket.Send(connection);
        }

        private static void OnC2SAskServerSelect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskServerSelect(buffer,connection);
            int userID = CCM.GetClientConnection(connection).UserID;
            string ip = "127.0.0.1";
            int port = 8000;
            //AgentConnection.connection.SendObject("UserEnterCharSelect",userID);
            var outPacket = new AuthPackets.S2CAnsServerSelect(userID, ip, port);
            outPacket.Send(connection);
        }
    }
}
