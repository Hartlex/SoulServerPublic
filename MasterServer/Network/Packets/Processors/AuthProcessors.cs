using System.Configuration;
using KaymakNetwork;
using MasterServer.Clients;
using MasterServer.Database;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace MasterServer.Network.Packets.Processors
{
    internal static class AuthProcessors
    {
        internal static void OnC2SAskConnect(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new AuthPackets.C2SAskConnect(buffer, connection);
            //Get Protocol Version
            var protocolBytes = incPacket.ProtocolVersion;
            var protocol = protocolBytes[0] + "." + protocolBytes[1] + "." + protocolBytes[2];

            var address = incPacket.serverAddress; //Could also be Client Address
            //Check if Protocol Versions Match
            var canConnect = (protocol == ConfigurationManager.AppSettings["protocolVersion"]);
            var outPacket = new AuthPackets.S2CAnsConnect(canConnect);
            outPacket.Send(connection);
        }
        internal static void OnC2SAskLogin(ByteBuffer buffer, Connection connection)
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

        internal static void OnC2SAskForServerList(ByteBuffer buffer, Connection connection)
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

        internal static void OnC2SAskServerSelect(ByteBuffer buffer, Connection connection)
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
    }
}
