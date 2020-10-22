using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SunCommon.PacketStructs;

namespace SunCommon.Entities.Map
{
    public class SunMap
    {
        public ushort mapCode;
        public ushort mapKind;
        public SunVector wayPointPosition;
        private List<ushort> WayPointPortalList= new List<ushort>();

        public SunMap(ushort mapCode, SunVector wayPointPosition, List<ushort> WayPointPortalList)
        {
            this.mapCode = mapCode;
            this.wayPointPosition = wayPointPosition;
            this.WayPointPortalList = WayPointPortalList;
        }

        public ushort getWaypointPortal(int index)
        {
            return WayPointPortalList[index-1];
        }
    }
}
