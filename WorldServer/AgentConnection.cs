using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;

namespace WorldServer
{
    static class AgentConnection
    {
        public static Connection connection;
        public static bool isConnected = false;

        public static void IniitializeConnection(string address, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            ConnectionInfo info = new ConnectionInfo(address.ToString(), port, ApplicationLayerProtocolStatus.Enabled);
            Connection conn = TCPConnection.GetConnection(info, optionsToUse);
            conn.AppendIncomingPacketHandler<string>("WorldAgentConnectionACK", (header, connection, content) =>
                {
                    AgentConnection.connection = conn;
                    isConnected = true;
                    Console.WriteLine("Agent Server connected!");
                    RegisterAgentPackets();
                }
            );
            conn.SendObject("WorldAgentConnection");
        }

        private static void RegisterAgentPackets()
        {

        }
    }
}