using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities.Item;
using static SunCommon.PacketStructs;

namespace SunCommon.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public long Money { get; set; }
        public byte InventoryLock { get; set; }
        public byte[] InventoryItem { get; set; }
        public byte[] TmpInventoryItem { get; set; }
        public byte[] EquipItem { get; set; }
        public ItemSlotInfo[] invSlotsInfo = new ItemSlotInfo[75];
        public ItemSlotInfo[] equipInfo = new ItemSlotInfo[17];
        public ItemSlotInfo[] tempInventory = new ItemSlotInfo[20];
        
        public Inventory()
        {
            //SerializeInventoryByteStream();
        }

        public void SerializeInventoryByteStream()
        {
            var equipCount = EquipItem[0];
            if (EquipItem.Length != equipCount * 16 + 1) return; //SlotInfo Size
            for (int i = 0; i < equipCount; i++)
            {
               equipInfo[i]=new ItemSlotInfo(ByteUtils.SlicedBytes(EquipItem,i+1,(i+1)*16+1));
            }
            var tempInventoryCount = TmpInventoryItem[0];
            if (TmpInventoryItem.Length != tempInventoryCount * 16 + 1) return; //SlotInfo Size
            for (int i = 0; i < tempInventoryCount; i++)
            {
                tempInventory[i] = new ItemSlotInfo(ByteUtils.SlicedBytes(TmpInventoryItem, i + 1, (i + 1) * 16 + 1));
            }
            var inventoryCount = InventoryItem[0];
            if (invSlotsInfo.Length != inventoryCount * 16 + 1) return; //SlotInfo Size
            for (int i = 0; i < inventoryCount; i++)
            {
                invSlotsInfo[i] = new ItemSlotInfo(ByteUtils.SlicedBytes(InventoryItem, i + 1, (i + 1) * 16 + 1));
            }
            
        }
        
    }
}
