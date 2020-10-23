using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities.Map
{
    public class Portal
    {
        public uint id;
        public byte MapType;
        public byte MoveType;
        public ushort mapFrom;
        public ushort fieldFrom;
        public string areaFrom;
        public ushort mapTo;
        public ushort fieldTo;
        public string areaTo;
        public byte minLvl;
        public byte maxLvl;
        public ushort missionCount;
        public ushort questCode;
        public ushort itemCode;
        public byte itemNum;
        public uint wasteItem;
        public uint heim;

        public Portal(string[] s)
        {
            id=UInt32.Parse(s[0]);
            MapType = Byte.Parse(s[1]);
            MoveType = Byte.Parse(s[2]);
            mapFrom = UInt16.Parse(s[3]);
            fieldFrom = UInt16.Parse(s[4]);
            areaFrom = s[5];
            mapTo = UInt16.Parse(s[6]);
            fieldTo = UInt16.Parse(s[7]);
            areaTo = s[8];
            minLvl = Byte.Parse(s[9]);
            maxLvl = Byte.Parse(s[10]);
            missionCount = UInt16.Parse(s[11]);
            questCode = UInt16.Parse(s[12]);
            itemCode = UInt16.Parse(s[13]);
            itemNum = Byte.Parse(s[14]);
            wasteItem = UInt32.Parse(s[15]);
            heim = UInt32.Parse(s[16]);
        }
    }
}
