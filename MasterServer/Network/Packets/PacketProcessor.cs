using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using KaymakNetwork;
using MasterServer.Clients;
using MasterServer.Database;
using MasterServer.Properties;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Entities;
using SunCommon.Entities.Item;
using SunCommon.Packet.Agent.Character;
using SunCommon.Packet.Agent.CharacterStatus;
using SunCommon.Packet.Agent.Item;
using static MasterServer.Network.Packets.Processors.AuthProcessors;
using static MasterServer.Network.Packets.Processors.CharacterProcessors;
using static MasterServer.Network.Packets.Processors.CharacterStatusProcessors;
using static MasterServer.Network.Packets.Processors.ConnectionProcessors;
using static MasterServer.Network.Packets.Processors.InventoryProcessors;
using static MasterServer.Network.Packets.Processors.ItemProcessors;
using static MasterServer.Network.Packets.Processors.SyncProcessors;

namespace MasterServer.Network.Packets
{
    internal static class PacketProcessor   
    {
        public static Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>> AllPackets = new Dictionary<PacketCategory, Dictionary<int, Action<ByteBuffer, Connection>>>();
        public static bool FindPacketAction(PacketCategory category, int protocol, out Action<ByteBuffer, Connection> action)
        {
            action = null;
            return AllPackets.TryGetValue(category, out var Category) && Category.TryGetValue(protocol, out action);
        }
        public static void Initialize()
        {
            Console.WriteLine(Resources.PacketProcessor_Initialize_Load);
            InitializeCategories();
            InitializeProtocols();
            Console.WriteLine(Resources.PacketProcessor_Initialize_Success);
        }

        private static void InitializeCategories()
        {
            Dictionary<int, Action<ByteBuffer, Connection>> authActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AuthPackets, authActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load,PacketCategory.AuthPackets.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> connectionActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentConnection, connectionActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentConnection.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> characterActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentCharacter, characterActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentCharacter.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> syncActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.AgentSync, syncActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.AgentSync.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> characterStatusActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.CharacterStatus, characterStatusActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.CharacterStatus.ToString());

            Dictionary<int,Action<ByteBuffer,Connection>> itemActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.Item,itemActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.Item.ToString());

            Dictionary<int, Action<ByteBuffer, Connection>> inventoryActions = new Dictionary<int, Action<ByteBuffer, Connection>>();
            AllPackets.Add(PacketCategory.Inventory, inventoryActions);
            Console.WriteLine(Resources.PacketProcessor_InitializeCategories__load, PacketCategory.Inventory.ToString());
        }

        private static void InitializeProtocols()
        {
            InitAuthPackets();
            InitConnectionPackets();
            InitCharacterPackets();
            InitSyncPackets();
            InitCharacterStatusPackets();
            InitItemPackets();
            InitInventoryPackets();
        }

        private static void InitInventoryPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.Inventory, out var inventoryActions)) return;
            inventoryActions.Add(68, OnC2SOpenInventory);
        }

        private static void InitItemPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.Item, out var itemActions)) return;
            itemActions.Add(149,OnC2SAskBuyItem);
            itemActions.Add(211,OnC2sAskItemMove);
            itemActions.Add(57,OnC2SAskItemSplit);
            itemActions.Add(87, OnC2SAskItemMerge);
            itemActions.Add(187,OnC2SAskDeleteItem);
        }
        private static void InitCharacterStatusPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.CharacterStatus, out var characterStatusActions)) return;
            characterStatusActions.Add(60,OnC2SAskAttributeIncrease);
        }
        private static void InitAuthPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AuthPackets, out var authActions)) return;
            authActions.Add(1, OnC2SAskConnect);
            authActions.Add(3, OnC2SAskLogin);
            authActions.Add(15, OnC2SAskForServerList);
            authActions.Add(19, OnC2SAskServerSelect);
        }
        private static void InitConnectionPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentConnection, out var connectionActions)) return;
            connectionActions.Add(118,OnC2SAskEnterCharSelect);
            connectionActions.Add(31, OnC2SAskEnterGame);
            connectionActions.Add(223, OnC2SAskPrepareWorld);
            connectionActions.Add(40, OnC2SErrorMessage);
            connectionActions.Add(216,OnC2SAskBackToCharSelect);
        }
        private static void InitCharacterPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentCharacter, out var characterActions)) return;
            characterActions.Add(51,OnC2SAskSelectPlayer);
            characterActions.Add(111, OnC2SAskCreateCharacter);
            characterActions.Add(137, OnC2SAskDeleteCharacter);
            characterActions.Add(81, OnC2SAskDuplicateName);
        }
        private static void InitSyncPackets()
        {
            if (!AllPackets.TryGetValue(PacketCategory.AgentSync, out var syncActions)) return;
            syncActions.Add(141,OnC2SAskEnterWorld);
            syncActions.Add(115, OnC2SAskJumpMove);
            syncActions.Add(202, OnC2SAskMouseMove);
            syncActions.Add(43, OnC2SAskKeyboardMove);
            syncActions.Add(69,OnC2SSyncNewPosition);
            syncActions.Add(123, OnC2SSyncMoveStop);
            syncActions.Add(96,OnC2sSyncMaptile);
        }





    }
}