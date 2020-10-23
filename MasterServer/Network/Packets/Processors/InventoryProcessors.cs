using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon.Packet.Agent.Inventory;
using SunCommon.Packet.Agent.Item;

namespace MasterServer.Network.Packets.Processors
{
    internal static class InventoryProcessors
    {
        internal static void OnC2SOpenInventory(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new InventoryPackets.C2SOpenInventory();
            var inventory = ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;
            var outPacket = new ItemPackets.S2CAnsBuyItem(inventory.Money,inventory.inventoryItemCount,inventory.invSlotsInfo);
            //outPacket.Send(connection);
        }
    }
}
