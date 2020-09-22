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

namespace AgentServer
{
    static class NetworkConfig
    {
        public static void InitializeNetwork()
        {
            RegisterPacketHandler();
            StartListening("127.0.0.1", 8000); // Client
            StartListeningManaged("127.0.0.1",6001); //World
            RegisterOnConnectHandler();
        }
        private static void RegisterPacketHandler()
        {
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler((header, connection, array) =>
            {
                PacketParser.ParseUnmanagedPacket(array, connection);
            }
            );
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("WorldAgentConnection", (header, connection, content) =>
            {
                connection.SendObject("WorldAgentConnectionACK");
                WorldServerConnection.connection = connection;
                WorldServerConnection.RegisterPackets();
            } );

        }
        private static void StartListening(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<NullSerializer>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            var listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Disabled);
            Connection.StartListening(listener, iPEndPoint);
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        private static void StartListeningManaged(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            var listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Enabled);
            Connection.StartListening(listener, iPEndPoint);
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        private static void RegisterOnConnectHandler()
        {
            NetworkComms.AppendGlobalConnectionEstablishHandler((connection) => {
                onConnect(connection);
            });
        }
        private static void onConnect(Connection connection)
        {
            //CCM.GetClientConnection()
        }
    }
}
