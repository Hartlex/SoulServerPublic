using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities.Item;
using static SunCommon.PacketStructs;

namespace SunCommon.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public ulong Money { get; set; }
        public byte InventoryLock { get; set; }
        public byte[] InventoryItem { get; set; }
        public byte[] TmpInventoryItem { get; set; }
        public byte[] EquipItem { get; set; }
        public ItemSlotInfo[] invSlotsInfo = new ItemSlotInfo[75];
        public int inventoryItemCount;
        public ItemSlotInfo[] equipInfo = new ItemSlotInfo[17];
        public int equipItemCount;
        public ItemSlotInfo[] tempInventory = new ItemSlotInfo[20];
        public int tempInventoryItemCount;

        public void SerializeInventoryByteStream()
        {
            equipItemCount = EquipItem[0];
            if (EquipItem.Length != equipItemCount * 28 + 1) return; //SlotInfo Size
            for (int i = 0; i < equipItemCount; i++)
            {
                equipInfo[i] = new ItemSlotInfo(ByteUtils.SlicedBytes(EquipItem, i*28 + 1, (i + 1) * 28 + 1));
                
            }

            tempInventoryItemCount = TmpInventoryItem[0];
            if (TmpInventoryItem.Length != tempInventoryItemCount * 28 + 1) return; //SlotInfo Size
            for (int i = 0; i < tempInventoryItemCount; i++)
            {
                tempInventory[i] = new ItemSlotInfo(ByteUtils.SlicedBytes(TmpInventoryItem, i*28 + 1, (i + 1) * 28 + 1));
            }

            inventoryItemCount = InventoryItem[0];
            if (InventoryItem.Length != inventoryItemCount * 28 + 1) return; //SlotInfo Size
            for (int i = 0; i < inventoryItemCount; i++)
            {
                invSlotsInfo[i] = new ItemSlotInfo(ByteUtils.SlicedBytes(InventoryItem, i*28 + 1, (i + 1) * 28 + 1));
            }
        }

        public void DeserializeInventoryToByteStream()
        {
            var inventoryBytes = new List<byte>();
            inventoryBytes.Add((byte)inventoryItemCount);
            foreach (var slotInfo in invSlotsInfo)
            {
                if (slotInfo != null) inventoryBytes.AddRange(slotInfo.ToBytes());
            }

            InventoryItem = inventoryBytes.ToArray();

            var tmpinventoryBytes = new List<byte>();
            tmpinventoryBytes.Add((byte)tempInventoryItemCount);
            foreach (var slotInfo in tempInventory)
            {
                if (slotInfo != null) tmpinventoryBytes.AddRange(slotInfo.ToBytes());
            }

            TmpInventoryItem = tmpinventoryBytes.ToArray();

            var equipBytes = new List<byte>();
            equipBytes.Add((byte)equipItemCount);
            foreach (var slotInfo in equipInfo)
            {
                if (slotInfo != null) equipBytes.AddRange(slotInfo.ToBytes());
            }

            EquipItem = equipBytes.ToArray();


        }
        public bool AddItemToInv(SunItem item, out ItemSlotInfo slotInfo)
        {
            slotInfo = null;
            if (isItemInInventory(item, out var slot))
            {
                slot.itemInfo.itemCount++;
                slotInfo = slot;
                return true;
            }
            var slotNum = FindFreeInvSlot();
            if (slotNum == -1) return false;
            invSlotsInfo[slotNum]=new ItemSlotInfo((byte)slotNum,item,1);
            slotInfo = invSlotsInfo[slotNum];
            inventoryItemCount++;
            return true;
        }
        private bool isItemInInventory(SunItem item,out ItemSlotInfo itemSlot)
        {
            itemSlot = null;
            foreach (var slot in invSlotsInfo)
            {
                if (slot != null && (uint) BitConverter.ToUInt16(slot.itemInfo.code, 0) == item.itemId)
                {
                    itemSlot = slot;
                    return true;
                }
            }

            return false;
        }

        private int FindFreeInvSlot(byte slotId=1 )
        {
            var slots = GetSlots(slotId);
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null) return i;
            }

            return -1;
        }

        public bool MoveItem(byte slotIdFrom, byte slotIdTo, byte positionFrom, byte positionTo, byte unk1)
        {
            var slots1 = GetSlots(slotIdFrom);
            var slots2 = GetSlots(slotIdTo);

            var slot1Pos = getArrayPosition(slots1, positionFrom);
            var slot2Pos = getArrayPosition(slots2, positionTo);

            if (slot2Pos == -1)
            {
                slots1[slot1Pos].position = positionTo;
                slot2Pos = FindFreeInvSlot(slotIdTo);
                slots2[slot2Pos] = slots1[slot1Pos];
                slots1[slot1Pos] = null;
            }
            else
            {
                slots1[slot1Pos].position = positionTo;
                slots2[slot2Pos].position = positionFrom;

                var tmp = slots2[slot2Pos];
                slots2[slot2Pos] = slots1[slot1Pos];
                slots1[slot1Pos] = tmp;
            }

            CalculateAllItemCounts();


            return true;
            //if (!TryGetSlotInfo(positionFrom, GetSlots(slotIdFrom), out var slotFrom)) return false;
            //if (TryGetSlotInfo(positionTo, GetSlots(slotIdTo), out var slotTo))
            //{
            //    slotFrom.position = positionTo;
            //    slotTo.position = positionFrom;
            //    return true;
            //}

            //slotFrom.position = positionTo;

            //return true;

        }

        private void CalculateAllItemCounts()
        {
            inventoryItemCount = CalculateItemCounts(invSlotsInfo);
            equipItemCount = CalculateItemCounts(equipInfo);
            tempInventoryItemCount = CalculateItemCounts(tempInventory);
        }
        private int CalculateItemCounts(ItemSlotInfo[] slots)
        {
            int counter = 0;
            foreach (var slot in slots)
            {
                if (slot != null) counter++;
            }

            return counter;
        }

        public bool SplitItem(byte posFrom, byte posTo, byte amountLeft, byte amountMove)
        {
            TryGetSlotInfo(posFrom, GetSlots(1), out var orgSlot);
            
            var newSlot = new ItemSlotInfo(orgSlot.ToBytes());
            newSlot.position = posTo;
            newSlot.itemInfo.itemCount = amountMove;
            var index = FindFreeInvSlot();
            GetSlots(1)[index] = newSlot;

            orgSlot.itemInfo.itemCount = amountLeft;
            inventoryItemCount++;
            return true;

        }

        private bool TryGetSlotInfo(byte pos,ItemSlotInfo[] slots, out ItemSlotInfo slotInfo)
        {
            slotInfo = null;
            foreach (var slot in slots)
            {
                if(slot ==null) continue;
                if ((byte) slot.position != pos) continue;
                slotInfo = slot;
                return true;
            }

            return false;
        }

        private int getArrayPosition(ItemSlotInfo[] slots, byte pos)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i]==null) continue;
                if ((byte) slots[i].position == pos) return i;
            }

            return -1;
        }

        private ItemSlotInfo[] GetSlots(byte slotIndex)
        {
            switch (slotIndex)
            {
                case 1:
                    return invSlotsInfo;
                case 2:
                    return equipInfo;
                default:
                    return null;
            }
        }

        public bool DeleteItem(byte pos)
        {
            for (int i = 0; i < invSlotsInfo.Length; i++)
            {
                if (invSlotsInfo[i] == null) continue;
                if ((byte) invSlotsInfo[i].position == pos) invSlotsInfo[i] = null;
            }
            
            inventoryItemCount--;
            return true;
        }
    }
}