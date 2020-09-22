using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
