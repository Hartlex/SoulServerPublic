using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon
{
    public static class ByteUtils
    {
        public static byte[] ToByteArray(int i, int size)
         {
            byte[] newbytes = new byte[size];
            var intbytes = BitConverter.GetBytes(i);
            var count = intbytes.Length > size ? size : intbytes.Length;
            Buffer.BlockCopy(intbytes, 0, newbytes, 0, count);
            return newbytes;
        }

        public static byte[] ToByteArray(string str, int size)
        {
            byte[] ret = new byte[size];
            Array.Copy(Encoding.ASCII.GetBytes(str), ret, str.Length);
            return ret;
        }

        public static byte[] ToByteArray(string str)
        {
            List<byte> newBytes = new List<byte>();
            for (int i = 1; i < str.Length; i++)
            {
                if (i % 2 == 0)
                {
                    var substr = str.Substring(i - 2, 2);
                    var j = Int32.Parse(substr, System.Globalization.NumberStyles.HexNumber);
                    newBytes.Add((byte)j);
                }
            }
            return newBytes.ToArray();

        }
        public static byte[] ToByteArray(float f, int size)
        {
            var newBytes = new byte[size];
            var fbytes = BitConverter.GetBytes(f);
            var count = fbytes.Length > size ? size : fbytes.Length;
            Buffer.BlockCopy(fbytes,0,newBytes,0,count);
            return newBytes;
        }
        public static int ToInt(byte[] bytes)
        {
            return Convert.ToInt32(bytes);
        }

        public static int ToInt(byte b)
        {
            return Convert.ToInt32(b);
        }

        public static byte[] SlicedBytes(byte[] bytes, int from, int to)
        {
            var newBytes = new byte[to - from];
            var j = 0;
            for (var i = from; i < to; i++, j++)
            {
                if (i >= bytes.Length) break;
                newBytes[j] = bytes[i];
            }

            return newBytes;

        }

        public static sbyte[] SlicedSbytes(sbyte[] bytes, int from, int to)
        {
            var newBytes = new sbyte[to - from];
            var j = 0;
            for (var i = from; i < to; i++, j++)
            {
                if (i >= bytes.Length) break;
                newBytes[j] = bytes[i];
            }

            return newBytes;
        } 
        public static byte[] cutTail(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0x00)
                {
                    byte[] newBytes = new byte[i];
                    Buffer.BlockCopy(bytes, 0, newBytes, 0, i);
                    return newBytes;
                }
            }
            return null;
        }
        public static sbyte[] cutTail(sbyte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 00)
                {
                    sbyte[] newBytes = new sbyte[i];
                    Buffer.BlockCopy(bytes, 0, newBytes, 0, i);
                    return newBytes;
                }
            }
            return null;
        }
        public static byte[] uintToByteArray(UInt32 v0, UInt32 v1)
        {
            var v0bytes = BitConverter.GetBytes(v0);
            var v1bytes = BitConverter.GetBytes(v1);
            var result = new byte[8];
            Buffer.BlockCopy(v0bytes, 0, result, 0, 4);
            Buffer.BlockCopy(v1bytes, 0, result, 4, 4);
            return result;
        }

        public static sbyte[] ToSbytes(byte[] bytes)
        {
            sbyte[] result = new sbyte[bytes.Length];
            Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
            return result;
        }


        public static byte[] ToByteArray(sbyte[] sbytes)
        {
            byte[] result = new byte[sbytes.Length];
            Buffer.BlockCopy(sbytes, 0, result, 0, sbytes.Length);
            return result;
        }
        public static sbyte[] uintToSByteArray(UInt32 v0, UInt32 v1)
        {
            var v0bytes = BitConverter.GetBytes(v0);
            var v1bytes = BitConverter.GetBytes(v1);
            var result = new sbyte[8];
            Buffer.BlockCopy(v0bytes, 0, result, 0, 4);
            Buffer.BlockCopy(v1bytes, 0, result, 4, 4);
            return result;
        }

        public static byte[] PacketLength(List<byte> packet)
        {
            int len = packet.Count;
            return ToByteArray(len,2);
            
        }
    }



}
