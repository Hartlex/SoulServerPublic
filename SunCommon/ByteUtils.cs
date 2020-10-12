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
            //str =
            //    "183bb227760000c4160000bbcd0843b093e84e04d9dab8d74d59775700f300030ce18c00bab3f33e696f00003a11213fc30018a7c981638c613636f0e4b20000fb5583cbfc939495b2c65f00521716aa39aa005d52ca4775062d260300f08602af15955663530026ec06003b151f3a00b2f48b0092156614af00af7ce40031b443a6d5e17500090ca8f55200194bb10c316d1041e566972204f1b2bdf534001484d90000e4317843c400308000d000e6f0d624102d8ebbfa590950b08500e12e367b0031809e000144512fce4215317a006d00f13f110103b79c9c54193b000514da3b2ffdc6ba3fc36123b36f793d09389189190e17a903c2109e1cf7a9a30002b3ecc86c3f5cb89a3e02dc410008c96c3f";
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
