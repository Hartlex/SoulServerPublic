using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Parser;

namespace SunCommon.Entities.Map
{
    public static class PortalManager
    {
        private static Dictionary<uint,Portal> portals = new Dictionary<uint, Portal>();

        public static void Initialize()
        {
            portals = MapPortalParser.LoadAllPortals();
        }

        public static Portal getPortal(uint id)
        {
            return portals.TryGetValue(id, out var portal) ? portal : null;
        }

        public static Portal getPortal(byte key1, byte key2)
        {
            var fromMap = getFromMap(key2,out key2);
            var map = MapManager.GetMap((ushort)fromMap);
            int index = generateIndex(key1, key2);
            var portalId = map.getWaypointPortal(index);
            return getPortal(portalId);


        }

        private static int generateIndex(byte key1, byte key2)
        {
            if (key1 < 50) key1 = 1;
            else if (key1 < 100) key1 = 2;
            else if (key1 < 200) key1 = 3;
            else if (key1 > 200) key1 = 4;

            return key2 * 4 + key1;
        }


        private static int getFromMap(byte key2, out byte newKey2)
        {
            newKey2 = (byte)(key2 % 25);
            switch (key2/25)
            {
                case 0: return 10001;
                case 1: return 10002;
                case 2: return 10003;
                case 3: return 10004;
                default: return 10001;
            }
        }

        private static uint getPortalId(int key)
        {
            switch (key)
            {
                case 1: return 1;
            }

            return 0;
        }

    }
}
