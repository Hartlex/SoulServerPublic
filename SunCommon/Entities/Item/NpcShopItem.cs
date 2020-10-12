using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public class NpcShopItem
    {
        private int itemId;
        private int byItemNum;
        private int IgType;

        public NpcShopItem(int itemId, int byItemNum, int igType)
        {
            this.itemId = itemId;
            this.byItemNum = byItemNum;
            this.IgType = igType;
        }

        public int GetItemId()
        {
            return itemId;
        }
    }
}
