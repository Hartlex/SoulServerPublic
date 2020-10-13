using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public class NpcShop
    {
        private int id;
        public Dictionary<int, NpcShopTab> npcShopTabs;

        public NpcShop(int id, Dictionary<int, NpcShopTab> tabs)
        {
            this.id = id;
            this.npcShopTabs = tabs;
        }

        public int GetItem(int page, short index)
        {
            return npcShopTabs[page].GetItem(index);
        }
    }
}
