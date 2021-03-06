﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;

namespace WorldServer
{
    class NetworkConfig
    {
        public static void InitializeNetwork()
        {
            RegisterPacketHandler();
            StartListening("127.0.0.1",8002 ); //Client
            RegisterOnConnectHandler();
        }
        private static void RegisterPacketHandler()
        {
            NetworkComms.AppendGlobalIncomingUnmanagedPacketHandler((header, connection, array) =>
                {
                    PacketParser.ParseUnmanagedPacket(array, connection);
                }
            );

        }
        private static void StartListening(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<NullSerializer>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            var listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Disabled);
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
            
        }
    }
}
