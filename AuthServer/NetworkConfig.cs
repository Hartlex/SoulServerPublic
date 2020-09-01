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

namespace AuthServer
{
    internal static class NetworkConfig
    {

        public static void InitialiseNetwork()
        {
            RegisterPacketHandler();
            StartListening("127.0.0.1", 44405);
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
                Console.WriteLine("\nReceived unmanaged byte[] from " + connection.ToString());

                for (int i = 0; i < array.Length; i++)
                    Console.Write(array[i].ToString()+"|");
            });
        }
        private static void RegisterOnConnectHandler()
        {
            NetworkComms.AppendGlobalConnectionEstablishHandler((connection) => {
                onConnect(connection);
            });
        }
        private static void onConnect(Connection connection)
        {
            Console.WriteLine("Sending Hello Packet");
            connection.SendUnmanagedBytes(Packets.HelloPacket());
        }
        #endregion

    }
}
