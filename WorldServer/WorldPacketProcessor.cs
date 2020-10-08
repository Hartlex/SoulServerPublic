using System;
using System.Collections.Generic;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;
using SunCommon;

namespace WorldServer
{
    internal static class WorldPacketProcessor
    {
        public static Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>> allPackets = new Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>>();

        public static void Initialize()
        {
            InitializeCategories();
            InitializeProtocols();
        }

        private static void InitializeProtocols()
        {
            
        }

        private static void InitializeCategories()
        {
        }
        
    }
}