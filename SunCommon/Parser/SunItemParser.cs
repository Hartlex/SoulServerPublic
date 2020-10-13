using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities.Item;

namespace SunCommon.Parser
{
    public static class SunItemParser
    {
        public static Dictionary<uint, SunItem> LoadItems(string path)
        {
            var items = new Dictionary<uint, SunItem>();
            var lines = ReadAllLines(path);
            foreach (var line in lines)
            {
                var item = ParseLine(line);
                items.Add(item.itemId, item);
            }

            return items;
        }

        private static SunItem ParseLine(string line)
        {
            var sl = line.Split('\t');
            var sllist = sl.ToList();
            sllist.RemoveAt(0);
            return new SunItem(sllist.ToArray());
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
    }
}
