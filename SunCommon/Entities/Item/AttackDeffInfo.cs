using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Item
{
    public class AttackDeffInfo
    {
        public Single phyMinDmg;
        public Single phyMaxDmg;
        public Single magMinDmg;
        public Single magMaxDmg;
        public Single phyDef;
        public Single magDef;
        public AttackDeffInfo() { }
        public AttackDeffInfo(string pmind, string pmaxd, string mmind, string mmaxd, string pd, string md)
        {
            phyMinDmg = Single.Parse(pmind);
            phyMaxDmg = Single.Parse(pmaxd);
            magMinDmg = Single.Parse(mmind);
            magMaxDmg = Single.Parse(mmaxd);
            phyDef = Single.Parse(pd);
            magDef = Single.Parse(md);
        }
    }
    
}
