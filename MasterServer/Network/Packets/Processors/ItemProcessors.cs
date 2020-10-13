using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using MasterServer.Clients;
using NetworkCommsDotNet.Connections;
using SunCommon;
using SunCommon.Entities.Item;
using SunCommon.Packet.Agent.Item;

namespace MasterServer.Network.Packets.Processors
{
    internal static class ItemProcessors
    {
        internal static void OnC2SAskBuyItem(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.C2SAskBuyItem(buffer);
            var itemId = NpcShopManager.GetItem(incPacket.unkId1, incPacket.shopPage, incPacket.itemIndex);
            var item = ItemManager.GetItem((uint) itemId);
            var character = ClientManager.GetClient(connection).GetSelectedCharacter();
            var ItemSlotInfo = new PacketStructs.ItemSlotInfo(0,item);
            character.Inventory.invSlotsInfo[0] = ItemSlotInfo;
            character.Inventory.Money -= item.ItemSellMoney;
            //var sb = new List<byte>();
            //sb.AddRange(new byte[]{01,00}); //invCount and tempInvCount
            //var slots = new PacketStructs.ItemSlotInfo[95];
            //for (int i = 0; i < slots.Length; i++)
            //{
            //    slots[i]=new PacketStructs.ItemSlotInfo();
            //}

            //slots[0] = ItemSlotInfo;
            //foreach (var slot in slots)
            //{
            //    sb.AddRange(slot.ToBytes());
            //}
            var outPacket = new ItemPackets.S2CAnsBuyItem(character.Inventory.Money, 1,0, character.Inventory.invSlotsInfo);
            outPacket.Send(connection);
        }
    }
}
