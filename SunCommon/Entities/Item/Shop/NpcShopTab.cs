using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public class NpcShopTab
    {
        private int id;
        private int shopId;
        private int lottoRatio;
        public Dictionary<int, NpcShopItem> items;

        public NpcShopTab(int shopId, int id, Dictionary<int, NpcShopItem> items, int lottoRatio = 0)
        {
            this.id = id;
            this.items = items;
            this.shopId = shopId;
        }

        public int GetId()
        {
            return id;
        }

        public int GetShopId()
        {
            return shopId;
        }

        public int GetItem(short index)
        {
            return items[index].GetItemId();
        }
    }
}
