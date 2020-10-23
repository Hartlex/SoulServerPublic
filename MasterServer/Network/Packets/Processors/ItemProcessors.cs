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
            if (inv.MoveItem(incPacket.slotIdFrom,incPacket.slotIdTo,incPacket.positionFrom,incPacket.positionTo,incPacket.amountToMove))
            {
                var outPacket = new ItemPackets.S2CAnsItemMove(incPacket.slotIdFrom, incPacket.slotIdTo, incPacket.positionFrom, incPacket.positionTo,incPacket.amountToMove);
                outPacket.Send(connection);
            }
        }

        internal static void OnC2SAskItemSplit(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.C2SAskItemSplit(buffer);
            var inv = ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;

            if (inv.SplitItem(incPacket.posFrom, incPacket.posTo, incPacket.amountLeft, incPacket.amountMove))
            {
                var outPacket = new ItemPackets.S2CAnsItemMove(1,1,incPacket.posFrom,incPacket.posTo,incPacket.amountMove);
                outPacket.Send(connection);
            }




        }

        internal static void OnC2SAskItemMerge(ByteBuffer buffer, Connection connection)
        {

            var incPacket =new ItemPackets.C2SAskItemMerge(buffer);
            var character = ClientManager.GetClient(connection).GetSelectedCharacter();
            var item = ItemManager.GetItem(4202);
            character.Inventory.AddItemToInv(item, out var slotInfo, 100);

        }

        internal static void OnC2SAskDeleteItem(ByteBuffer buffer, Connection connection)
        {
            //var incPacket = new ItemPackets.C2SAskItemDelete(buffer);
            //var inv= ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;
            //inv.DeleteItem(incPacket.pos);
            //var outPacket = new ItemPackets.S2CAnsBuyItem(inv.Money,inv.inventoryItemCount, inv.invSlotsInfo);
            //outPacket.Send(connection);
        }

        internal static void OnC2SAskEnchant(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.S2CAskEnchant(buffer);
            var outPacket = new ItemPackets.S2CAnEnchant(incPacket.pos,1);
            outPacket.Send(connection);
        }

        internal static void OnC2SAskItemBind(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.C2SAskItemBind(buffer);
            var inv = ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;
            //var sb = new byte[]
            //{
            //    02,00,
            //    33,0
            //};
            //connection.SendUnmanagedBytes(sb);
            var topos = (byte)0;
            inv.MoveItem(1, 2, incPacket.pos, topos, 0);
            var outPacket = new ItemPackets.S2CAnsItemMove(1, 2, incPacket.pos, topos, 0);
            //var outPacket = new ItemPackets.S2CAnsItemBind(incPacket.pos);
            outPacket.Send(connection);
        }

        internal static void OnC2SaskItemDropOld(ByteBuffer buffer, Connection connection)
        {
            var incPacket = new ItemPackets.C2SAskItemDrop(buffer);
            var pos = ClientManager.GetClient(connection).GetSelectedCharacter().CharacterPosition;
            var inv = ClientManager.GetClient(connection).GetSelectedCharacter().Inventory;
            var outPacket = new ItemPackets.S2CAnsItemDrop(incPacket.slotIndex,incPacket.pos);
            outPacket.Send(connection);
            var result = new List<byte>();
            
            var sb= new byte[]
            {
                253,93,
                00,00,00,01, // from monster
                00,00,00,254,
                59,00,00,00, //playerOwner
                1, //Field Item Type 0:item 1:money
                00,00,00,00,00,00,00,10 //money amount

            };
            result.AddRange(sb);
            var ap = inv.getArrayPosition(inv.invSlotsInfo, incPacket.pos);
            result.AddRange(inv.invSlotsInfo[ap].ToBytes());
            result.AddRange(new PacketStructs.SunVector(pos.LocationX,pos.LocationY,pos.LocationZ).GetBytes());


            result.InsertRange(0,BitConverter.GetBytes((ushort)result.Count));
            connection.SendUnmanagedBytes(result.ToArray());
            //var sb = new byte[]
            //{
            //    0xA0, 0x79, 0x16, 0x13, 0x00, 0x15, 0x5C, 0x4A, 0xA1, 0x0F, 0x01, 0x05,
            //    00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00
            //};
        }
        internal static void OnC2SaskItemDrop(ByteBuffer buffer, Connection connection)
        {
            //var incPacket = new ItemPackets.C2SAskItemDrop(buffer);
            //var character = ClientManager.GetClient(connection).GetSelectedCharacter();
            //var pos = character.CharacterPosition;
            //var result = new List<byte>();
            //var sb = new byte[]
            //{
            //    //0x0a, 0x00,
            //    //0xff, 0x01,
            //    //33, 0x00, 0x00, 0x00,
            //    //0x21, 0x13, 1, incPacket.pos,
            //    66,00,
            //    253, 93,
            //    0, 0, 0, 0, //from monster
            //    254, 0, 0, 0, //objKey
            //    33, 0, 0, 0,  //ownder
            //    0, //FieldItemType
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204,
            //    204, 204, 204, 204, 204
            //};
            //var posBytes = new PacketStructs.SunVector(pos.LocationX, pos.LocationY, pos.LocationZ).GetBytes();
            //result.AddRange(sb);
            //result.AddRange(posBytes);
            //connection.SendUnmanagedBytes(result.ToArray());
        }
    }
}
