using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SunCommon.PacketStructs;

namespace SunCommon.Entities.Map
{
    public static class MapManager
    {
        private static Dictionary<ushort, SunMap> maps = new Dictionary<ushort, SunMap>()
        {
            {10001,new SunMap(
                10001,
                new SunVector(-53.0f,-32.0f,-23.0f),
                new List<ushort>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16}
                )
            },
            {10002,new SunMap(
                10002,
                new SunVector(172.07f,141.59f,16.02f),
                new List<ushort>(){0,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116}
                ) },
            {10003,new SunMap(
                10003,
                new SunVector(337.22f,336.09f,39.61f),
                new List<ushort>(){0,201,202,203,204,205,206,207,208,209,210,211,212,213,214,215,216})  },
            {10004,new SunMap(
                10004,
                new SunVector(73.61f,-62.10f,3.01f),
                new List<ushort>(){0,301,302,303,304,305,306,307,308,309,310,311,312,313,314,315,316}
                )  },
            {
                20004,new SunMap(
                    20004,
                    new SunVector(468.5f,426.69f,59.3f),
                    null
                    )
            }
        };

        public static SunMap GetMap(ushort id)
        {
            return maps.TryGetValue(id, out var map) ? map : null;
        }
    }
}
