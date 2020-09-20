using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.DPSBase.SevenZipLZMACompressor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SunCommon;
using SunCommon.Entities;

namespace AuthServer
{
    internal static class DBConnection
    {
        public static Connection connection;
        public static bool isConnected = false;
        public static void IniitializeConnection(string address,int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            ConnectionInfo info = new ConnectionInfo(address.ToString(), port,ApplicationLayerProtocolStatus.Enabled);
            Connection conn = TCPConnection.GetConnection(info,optionsToUse,true);
            conn.AppendIncomingPacketHandler<string>("AuthDBConnectionACK", (header, connection, content) =>
             {
                 DBConnection.connection = conn;
                 isConnected = true;
                 Console.WriteLine("DBServer connected!");
                 RegisterAuthPackets();
             }
             );
            conn.SendObject("AuthDBConnection");


        }

        public static void RegisterAuthPackets()
        {
            connection.AppendIncomingPacketHandler<string[]>("UserLoginSucces", (header, connection, content) =>
            {
                ClientConnection conn = CCM.GetClientConnection(content[1]);
                conn.UserID= Int32.Parse(content[0]);
                var packet = new AuthPackets.S2CAnsLogin(true);
                packet.Send(conn.AuthConnection);
            });
        }
    }

}
