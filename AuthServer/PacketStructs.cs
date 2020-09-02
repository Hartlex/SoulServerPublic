using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer
{
    static class PacketStructs
    {

        public static byte[] stringBytes(string str,int size)
        {
            byte[] ret = new byte[size];
            Array.Copy(Encoding.ASCII.GetBytes(str), ret, str.Length);
            return ret;
        }
        public static byte[] intBytes(int[] ints,int size)
        {
            byte[] ret = new byte[size];
            for(int y = 0; y < ints.Length; y++)
            {
                ret[y] = Convert.ToByte(ints[y]);
            }
            //Array.Copy(Encoding.ASCII.GetBytes(i.ToString()),ret, i.ToString().Length);
            return ret;
        }
        public static byte sunBool(bool b)
        {
            return Convert.ToByte(!b);
        }
        public static byte[] length(List<byte> packet)
        {
            int len = packet.Count;
            return new byte[] { Convert.ToByte(len), 00 };
        }
        public static byte[] ServerInfo(string name, int number)
        {
            var nameBytes = stringBytes(name, 32);
            List<byte> packet = new List<byte>();
            packet.AddRange(nameBytes);
            packet.Add(00);
            packet.Add((byte)number);
            packet.Add(00);
            packet.Add(00);
            return packet.ToArray();
        }
        public static byte[] ChannelInfo(string name,int channelNr,int belongToServerNr)
        {
            var nameBytes = stringBytes(name, 32);
            List<byte> packet = new List<byte>();
            packet.AddRange(nameBytes);
            packet.Add(00); // unk
            packet.Add((byte)belongToServerNr);
            packet.Add((byte)channelNr);
            packet.Add(01); //unk cant be 00
            packet.Add(00); //separator always 00
            return packet.ToArray();
        }
    }
}
