using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Parser;
using static SunCommon.PacketStructs;

namespace SunCommon.Entities.Item
{
    public static class ItemManager
    {
        private static Dictionary<uint,SunItem> allItems = new Dictionary<uint, SunItem>();
        
        public static void Initialize()
        {
            var wasteItems = SunItemParser.LoadItems("C:\\SoulServer\\Data\\WasteItemInfo.txt");
            var weaponItems = SunItemParser.LoadItems("C:\\SoulServer\\Data\\WeaponItemInfo.txt");
            var armorItems = SunItemParser.LoadItems("C:\\SoulServer\\Data\\ArmorItemInfo.txt");
            wasteItems.ToList().ForEach(x => allItems.Add(x.Key, x.Value));
            weaponItems.ToList().ForEach(x => allItems.Add(x.Key, x.Value));
            armorItems.ToList().ForEach(x => allItems.Add(x.Key, x.Value));
        }

        public static SunItem GetItem(uint id)
        {
            return allItems.TryGetValue(id, out var item) ? item : null;
        }

        public static ItemSlotInfo GetItemSlotInfo(SunItem item, byte position)
        {
            return new ItemSlotInfo(position,item);
        }
    }
}
