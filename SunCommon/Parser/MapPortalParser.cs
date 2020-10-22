using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities.Map;

namespace SunCommon.Parser
{
    public static class MapPortalParser
    {

        private static string path = "C:\\SoulServer\\Data\\MapEnterancePortal.txt";
        public static Dictionary<uint,Portal> LoadAllPortals()
        {
            var lines = ReadAllLines(path);
            var result = new Dictionary<uint,Portal>();
            foreach (var line in lines)
            {
                var portal = ParseLine(line);
                result.Add(portal.id,portal);
            }

            return result;
        }
        private static Portal ParseLine(string line)
        {
            var sl = line.Split('\t').ToList();
            sl.RemoveAt(0);
            sl.RemoveRange(17,sl.Count-17);
            return new Portal(sl.ToArray());
        }
        private static List<string> ReadAllLines(string path)
        {
            var allLines = File.ReadAllLines(path);
            List<string> noCommentLines = new List<string>();
            foreach (var line in allLines)
            {
                if (!line.StartsWith("//")) noCommentLines.Add(line);
            }
            noCommentLines.RemoveAt(0);
            return noCommentLines;
        }
        //TODO put this in File and connect with tileIds
        public static Dictionary<uint,PacketStructs.SunVector> portalOutPositions = new Dictionary<uint, PacketStructs.SunVector>()
        {
            {1,new PacketStructs.SunVector(172.07f,141.59f,16.02f)}
        };
    }
}
