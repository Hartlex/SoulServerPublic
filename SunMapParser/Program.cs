using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunMapParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"H:\OldSunDev\Release\data\Map\Object\Map01\Map01_F01_Boss.map",Encoding.UTF8);
            List<byte[]> newlines = new List<byte[]>();
            int i = 0;
            foreach (var line in lines)
            {
                newlines.Add(Encoding.ASCII.GetBytes(lines[i]));
                i++;
            }
        }
    }
}
