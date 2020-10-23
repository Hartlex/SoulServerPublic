using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MasterServer.Clients;
using MasterServer.Network.Packets;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;

namespace MasterServer
{
    public class SunChannel
    {
        private SunServer server;
        public string name;
        public int worldPort;
        private List<Client> connectedClients = new List<Client>();
        private TCPConnectionListener listener;

        public SunChannel(string name, SunServer server, int worldPort)
        {
            this.name = name;
            this.server = server;
            this.worldPort = worldPort;
            InitializeChannel();
        }

        private void InitializeChannel()
        {
            StartListening("127.0.0.1", worldPort);
        }

        public void Shutdown()
        {
            Connection.StopListening(listener);
            NetworkComms.Shutdown();
        }
        private void StartListening(string ipAdress, int port)
        {
            SendReceiveOptions optionsToUse = new SendReceiveOptions<NullSerializer>();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            listener = new TCPConnectionListener(optionsToUse, ApplicationLayerProtocolStatus.Disabled);
            listener.AppendIncomingUnmanagedPacketHandler((header, connection, array) =>
            {
                PacketParser.ParseUnmanagedPacket(array, connection);
            });
            Connection.StartListening(listener, iPEndPoint);
            Console.WriteLine("Started Listening from IP:" + iPEndPoint.Address + " on Port:" + iPEndPoint.Port);
        }
        public void ConnectToChannel(Client client)
        {
            client.ConnectToChannel(this);
            connectedClients.Add(client);
        }
    }
}
