using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public static class NpcShopManager
    {
        public static Dictionary<int,NpcShop> npcShops = new Dictionary<int, NpcShop>();

        public static void Add(int id,NpcShop shop)
        {
            npcShops.Add(id,shop);
        }

        public static void AddTab(NpcShopTab tab)
        {
            int id = tab.GetId();
            if (npcShops.TryGetValue(tab.GetShopId(), out var npcShop))
            {
                npcShop.npcShopTabs.Add(tab.GetId(),tab);
            }
            else
            {
                Add(tab.GetShopId(),new NpcShop(id,new Dictionary<int, NpcShopTab>()));
                AddTab(tab);
            }
        }

        public static int GetItem(int shopId, int page, short index)
        {
            return npcShops[shopId].GetItem(page, index);
        }

        public static void Shutdown()
        {
            npcShops.Clear();
        }
    }
}
