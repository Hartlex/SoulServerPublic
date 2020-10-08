using System;
using System.Configuration;
using System.Net;
using MasterServer.Clients;
using MasterServer.Network.Packets;
using MasterServer.Properties;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using SunCommon;

namespace MasterServer.Network
{
    internal static class NetworkConfig
    {
        private static TCPConnectionListener listener;
        public static void Initialize()
        {
            Console.WriteLine(Resources.NetworkConfig_InitialiseNetwork_Load);
            RegisterPacketHandler();
            Console.WriteLine(Resources.NetworkConfig_InitialiseNetwork_Try_to_load_NetworkConfig___);
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["serverPort"]);
            Console.WriteLine(Resources.NetworkConfig_InitialiseNetwork_Successfully_loaded_NetworkConfig_);
            StartListening(ip, port); //Client
            RegisterOnConnectHandler();
            Console.WriteLine(Resources.NetworkConfig_InitialiseNetwork_Success);
        }

        public static void Stop()
        {

            StopListening();
            PacketParser.Unload();
        }

        private static void StopListening()
        {
            Connection.StopListening(listener);
            Console.WriteLine(Resources.NetworkConfig_StopListening_Stop+listener.LocalListenEndPoint);
        }

        #region PrivateMethods
        private static void StartListening(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<NullSerializer>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Disabled);
            listener.AppendIncomingUnmanagedPacketHandler((header, connection, array) =>
            {
                PacketParser.ParseUnmanagedPacket(array,connection);
            });
            Connection.StartListening(listener, iPEndPoint);
            
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        private static void RegisterPacketHandler()
        {
            PacketParser.Initialize();
            //NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler((header, connection, array) =>
            //{
            //    PacketParser.ParseUnmanagedPacket(array, connection);
            //}
            //);

        }
        private static void RegisterOnConnectHandler()
        {
            NetworkComms.AppendGlobalConnectionEstablishHandler(OnConnect);

        }
        private static void OnConnect(Connection connection)
        {
            ClientManager.UpdateOrAddClient(connection, out var client);
            var packet = new AuthPackets.S2CHelloPacket(client.EncryptionKey);
            packet.Send(connection);
            
            //Console.WriteLine(connection.ConnectionInfo.RemoteEndPoint);
            //sbyte[] encKey = ByteUtils.ToSbytes(BitConverter.GetBytes(EncryptionManager.generateEncKey()));
            //CCM.AddCC(connection, encKey);
            //var packet = new AuthPackets.S2CHelloPacket(encKey);
            //packet.Send(connection);
        }
        #endregion
    }
}
