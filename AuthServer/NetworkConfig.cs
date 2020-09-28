using System;
using NetworkCommsDotNet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections.TCP;
using SunCommon;

namespace AuthServer
{
    internal static class NetworkConfig
    {

        public static void InitialiseNetwork()
        {
            RegisterPacketHandler();
            StartListening("127.0.0.1", 44405); //Client
            //StartListening("127.0.0.1", 8001);  //AgentServer
            RegisterOnConnectHandler();
        }

        #region PrivateMethods
        private static void StartListening(string ipAdress,int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<NullSerializer>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress),port);
            var listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Disabled);
            Connection.StartListening(listener, iPEndPoint);
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        private static void RegisterPacketHandler()
        {
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler((header, connection, array) =>
            {
                PacketParser.ParseUnmanagedPacket(array,connection);
                
            }
            );
            //NetworkComms.AppendGlobalIncomingPacketHandler<string>("AgentAuthConnection",(
            //    (header, connection, incomingObject) =>
            //    {
            //        connection.SendObject("AgentAuthConnectionACK");
            //        AgentConnection.connection = connection;
            //        AgentConnection.RegisterPackets();
            //    } ));

        }
        private static void RegisterOnConnectHandler()
        {
            NetworkComms.AppendGlobalConnectionEstablishHandler((connection) => {
                onConnect(connection);
            });
        }
        private static void onConnect(Connection connection)
        {
            Console.WriteLine(connection.ConnectionInfo.RemoteEndPoint);
            //TODO generate rendom keys
            sbyte[] encKey = { 00, 00, 00, 00 };
            CCM.AddCC(connection,encKey);
            var packet = new AuthPackets.S2CHelloPacket(encKey);
            packet.Send(connection);
        }
        #endregion

    }
}
