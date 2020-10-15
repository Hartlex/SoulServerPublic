using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
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
            var inv = character.Inventory;
            character.BuyItem(item);






            var invCount = inv.inventoryItemCount;
            var money = inv.Money;
            var outPacket = new ItemPackets.S2CAnsBuyItem(money, invCount, character.Inventory.invSlotsInfo);
            outPacket.Send(connection);

            //var ItemSlotInfo = new PacketStructs.ItemSlotInfo(0, item, 1);
            //character.Inventory.invSlotsInfo[0] = ItemSlotInfo;
            //character.Inventory.Money -= item.ItemSellMoney;
            //var outPacket = new ItemPackets.S2CAnsBuyItem(character.Inventory.Money, 1, 0, character.Inventory.invSlotsInfo);
            //outPacket.Send(connection);
        }

        internal static void OnC2sAskItemMove(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.C2SAskItemMove(buffer);
            var inv = ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;
            if (inv.MoveItem(incPacket.slotIdFrom,incPacket.slotIdTo,incPacket.positionFrom,incPacket.positionTo,incPacket.unk))
            {
                var outPacket = new ItemPackets.S2CAnsItemMove(incPacket.slotIdFrom, incPacket.slotIdTo, incPacket.positionFrom, incPacket.positionTo, incPacket.unk);
                outPacket.Send(connection);
            }
        }
    }
}
