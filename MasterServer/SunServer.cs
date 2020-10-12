using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasterServer.Clients;
using MasterServer.Network.Packets;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;

namespace MasterServer
{
    public class SunServer
    {
        public int port;
        public string name;
        private TCPConnectionListener listener;
        public Dictionary<int,SunChannel> channels = new Dictionary<int, SunChannel>();
        private List<Client> connectedClients = new List<Client>();
        public SunServer(string name,int port)
        {
            this.name = name;
            this.port = port;
            InitializeSunServer();
        }

        public void AddChannel(string name, int port)
        {
            channels.Add(channels.Count+1,new SunChannel(name,this,port));
        }

        private void InitializeSunServer()
        {
            StartListening("127.0.0.1",port);
            var form1 = (Form1)Application.OpenForms[0];
            form1.AddServerBox("Etherain",null);

        }

        public void Shutdown()
        {
            Connection.StopListening(listener);
            foreach (var channel in channels)
            {
                channel.Value.Shutdown();
            }
        }
        private void StartListening(string ipAdress, int port)
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

        public void ConnectToServer(Client client)
        {
            client.ConnectToServer(this);
            connectedClients.Add(client);
        }
    }
}
