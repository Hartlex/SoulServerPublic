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

namespace DatabaseProxy
{
    class NetworkConfig
    {
        public static void InitialiseNetwork()
        {
            RegisterPacketHandler();
            StartListening("127.0.0.1", 7000);
            StartListening("127.0.0.1", 7001);
            RegisterOnConnectHandler();
        }

        private static void StartListening(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            var listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Enabled);
            Connection.StartListening(listener, iPEndPoint);
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        private static void RegisterPacketHandler()
        {
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler((header, connection, array) =>
            {
                //TODO Packet Handler
            }
            );
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("AuthDBConnection", (header, connection, content) => 
            {
                connection.SendObject("AuthDBConnectionACK");
                AuthConnection.connection = connection;
                AuthConnection.RegisterPackets();
            }
            );
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("AgentDBConnection", (header, connection, content) =>
                {
                    connection.SendObject("AgentDBConnectionACK");
                    AgentConnection.connection = connection;
                    AgentConnection.RegisterPackets();
                }
            );
            //NetworkComms.AppendGlobalIncomingPacketHandler<string[]>("UserLogin", (header, connection, content) =>
            //{
            //    AuthPackets.LoginPacket(content[0], content[1], content[2]);
            //});

        }
        private static void RegisterOnConnectHandler()
        {
            NetworkComms.AppendGlobalConnectionEstablishHandler((connection) => {
                onConnect(connection);
            });
        }
        private static void onConnect(Connection connection)
        {
            Console.WriteLine("Connection from Auth Received");
        }
    }
}
