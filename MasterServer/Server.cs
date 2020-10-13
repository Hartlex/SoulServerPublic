using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterServer.Clients;
using MasterServer.Network;
using MasterServer.Properties;
using NetworkCommsDotNet;
using SunCommon;
using SunCommon.Entities.Item;
using SunCommon.Parser;

namespace MasterServer
{
    internal static class Server
    {
        internal static Dictionary<int,SunServer> servers = new Dictionary<int, SunServer>();
        internal static void Start()
        {
            Console.WriteLine(Resources.Server_Start_Start);
            DatabaseManager.Initialize();
            NetworkConfig.Initialize();
            ClientManager.Initialize();
            NpcShopParser.Initialize();
            ItemManager.Initialize();
            InitializeServersAndChannels();
            Console.WriteLine(Resources.Server_Start_Success);
        }

        private static void InitializeServersAndChannels()
        {
            Console.WriteLine(Resources.Server_InitializeServerAndChannels_Load);

            var server1Name = ConfigurationManager.AppSettings["Server1Name"];
            int server1Port = Int32.Parse(ConfigurationManager.AppSettings["Server1Port"]);
            string channel1Name = ConfigurationManager.AppSettings["Channel1Name"];
            int channel1Port = Int32.Parse(ConfigurationManager.AppSettings["Channel1Port"]);

            var server1 = new SunServer(server1Name,server1Port);
            server1.AddChannel(channel1Name,channel1Port);
            servers.Add(1,server1);

        }

        public static SunServer getServer(int serverNumber)
        {
            return servers[serverNumber];
        }

        public static SunChannel getChannel(int serverNumber, int channelNumber)
        {
            return servers[serverNumber].channels[channelNumber];
        }
        public static List<PacketStructs.ServerInfo> getServerInfos()
        {
            var result = new List<PacketStructs.ServerInfo>();
            foreach (var server in servers)
            {
                result.Add(new PacketStructs.ServerInfo(server.Value.name,server.Key));
            }

            return result;
        }

        public static List<PacketStructs.ChannelInfo> getChannelInfos()
        {
            var result = new List<PacketStructs.ChannelInfo>();
            foreach (var server in servers)
            {
                foreach (var channel in server.Value.channels)
                {
                    result.Add(new PacketStructs.ChannelInfo(channel.Value.name,channel.Key,server.Key));
                }
            }

            return result;
        }
        internal static void Restart()
        {
            Stop();
            Start();
        }

        internal static void Stop()
        {
            Console.WriteLine(Resources.Server_Stop_Stop);
            ClientManager.Shutdown();
            NetworkConfig.Stop();
            foreach (var server in servers)
            {
                server.Value.Shutdown();
            }
            NetworkComms.CloseAllConnections();
            NetworkComms.Shutdown();
            servers.Clear();
            Console.WriteLine(Resources.Server_Stop_Success);
        }
    }
}
