using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon.Entities.Item;

namespace SunCommon.Parser
{
    public static class NpcShopParser
    {
        private static string path = "C:\\SoulServer\\Data\\ShopInfo.txt";

        public static void Initialize()
        {
            var lines = ReadAllLines(path);
            foreach (var line in lines)
            {
                ParseLine(line);
            }
        }

        private static void ParseLine(string line)
        {
            var splittedLine = line.Split('\t').ToList();
            var shopTab = GenerateShopTab(splittedLine);
            NpcShopManager.AddTab(shopTab);
        }

        private static NpcShopTab GenerateShopTab(List<string> values)
        {
            int shopId = Int32.Parse(values[0]);
            var name = values[1];
            int lotoRatio = Int32.Parse(values[2]);
            int id = Int32.Parse(values[3]);
            values.RemoveRange(0,4);
            var items = GenerateItemIds(values);
            //var tab = new NpcShopTab(shopId,id,);
            return new NpcShopTab(shopId,id,items);
        }

        private static Dictionary<int,NpcShopItem> GenerateItemIds(List<string> values)
        {
            var itemDict = new Dictionary<int,NpcShopItem>();
            int itemCount = 0;
            for (int i = 0; i < 75; i = i + 3)
            {
                var itemId = Int32.Parse(values[i]);
                if(itemId ==0) continue;
                var itemNum =Int32.Parse(values[i + 1]);
                var igtype = Int32.Parse(values[i + 2]);
                var item = new NpcShopItem(itemId,itemNum,igtype);
                itemDict.Add(itemCount,item);
                itemCount++;
            }

            return itemDict;
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
