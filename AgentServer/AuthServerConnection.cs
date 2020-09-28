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

namespace AgentServer
{
    public static class AuthServerConnection
    {
        public static Connection connection;
        public static bool isConnected = false;
        public static void IniitializeConnection(string address, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            ConnectionInfo info = new ConnectionInfo(address.ToString(), port, ApplicationLayerProtocolStatus.Enabled);
            Connection conn = TCPConnection.GetConnection(info, optionsToUse, true);
            conn.AppendIncomingPacketHandler<string>("AgentAuthConnectionACK", (header, connection, content) =>
                {
                    DBConnection.connection = conn;
                    isConnected = true;
                    Console.WriteLine("Auth Server connected!");
                    RegisterAuthPackets();
                }
            );
            conn.SendObject("AgentAuthConnection");


        }

        private static void RegisterAuthPackets()
        {
            connection.AppendIncomingPacketHandler<int>("UserEnterCharSelect",(
                (header, connection1, incomingObject) =>
                {

                }));
        }
    }
}
