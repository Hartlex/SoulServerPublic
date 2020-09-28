using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using SunCommon;
using static SunCommon.ByteUtils;


public static class TEA
    {
    public static sbyte[] EncSbytes(sbyte[] v, sbyte[] KEY)
        {
            var tempBytev8 = new byte[8];
            Buffer.BlockCopy(v, 0, tempBytev8, 0, 8);
            uint v0 = BitConverter.ToUInt32(SlicedBytes(tempBytev8, 0, 4), 0);
            uint v1 = BitConverter.ToUInt32(SlicedBytes(tempBytev8, 4, 8), 0);
            uint delta = 0x9e3779b9;
            uint sum = 0;
            var tempByteKey = new byte[4];
            Buffer.BlockCopy(KEY, 0, tempByteKey, 0, 4);
            for (int i = 0; i < 32; i++)
            {                         // basic cycle start
                sum += delta;
                v0 += ((v1 << 4) + tempByteKey[0]) ^ (v1 + sum) ^ ((v1 >> 5) + tempByteKey[1]);
                v1 += ((v0 << 4) + tempByteKey[2]) ^ (v0 + sum) ^ ((v0 >> 5) + tempByteKey[3]);
            }

            return uintToSByteArray(v0, v1);
        }

        public static sbyte[] passwordEncodeSBytes(String username,String passInput, sbyte[] keyInput)
        {
        //    var tempkey = new byte[4];
        //    Buffer.BlockCopy(keyInput, 0, tempkey, 0, 4);
        //    int keyValue = BitConverter.ToInt32(tempkey, 0);
        //    sbyte[] passMask = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //sbyte[] key = new sbyte[4];
        //key[0] = (sbyte)keyValue;
        //key[1] = (sbyte)(keyValue + 1);
        //key[2] = (sbyte)(keyValue + 2);
        //key[3] = (sbyte)(keyValue + 3);
        ////var key = keyInput;
        //var passBytes = Encoding.ASCII.GetBytes(passInput);
        //    Buffer.BlockCopy(passBytes, 0, passMask, 0, passBytes.Length);
        //    sbyte[] enc1 = EncSbytes(passMask, key);
        //    sbyte[] enc2 = EncSbytes(ByteUtils.SlicedSbytes(passMask, 8, 16), key);
        //    sbyte[] result = new sbyte[16];
        //    Buffer.BlockCopy(enc1, 0, result, 0, 8);
        //    Buffer.BlockCopy(enc2, 0, result, 8, 8);
        //    return result;
        sbyte[] passMask = new sbyte[16];
        sbyte[] key = new sbyte[4];
        sbyte[] result = new sbyte[16];

        try
        {
            // The max number is 23 because there is one separator byte between password and filler.
            var passBytes = Encoding.ASCII.GetBytes(passInput);
            sbyte[] filler = new sbyte[15 - Encoding.ASCII.GetBytes(passInput).Length];
            // Securely randomize the bytes to create a unique salt.
            //SecureRandom.getInstance("SHA1PRNG").nextBytes(filler);

            // Copy the passInput to passMask.
            Buffer.BlockCopy(passBytes,0,passMask,0,passBytes.Length);
            
            // Copy the filler to the end of passMask.
            //System.arraycopy(filler, 0, passMask, passInput.getBytes().length + 1, filler.length);
            Buffer.BlockCopy(filler,0,passMask,passBytes.Length+1,filler.Length);
            
           // Convert username to lowercase and to bytes.
            sbyte[] bUsername = ToSbytes(Encoding.ASCII.GetBytes(username.ToLower()));
            // Add the last character of the username to the end of the filler.
            passMask[15] = bUsername[bUsername.Length - 1];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        int keyValue = BitConverter.ToInt32(ToByteArray(keyInput),0);
        //int keyValue = Convert.ToInt32(keyInput);
        key[0] = (sbyte)keyValue;
        key[1] = (sbyte)(keyValue + 1);
        key[2] = (sbyte)(keyValue + 2);
        key[3] = (sbyte)(keyValue + 3);

        sbyte[] enc1 = EncSbytes(SlicedSbytes(passMask, 0, 8), key);
        sbyte[] enc2 = EncSbytes(SlicedSbytes(passMask, 8, 16), key);
        //sbyte[] enc3 = EncSbytes(ByteUtils.SlicedSbytes(passMask, 16, 23), key);

        Buffer.BlockCopy(enc1, 0, result, 0, 8);
        Buffer.BlockCopy(enc2, 0, result, 8, 8);
        //Buffer.BlockCopy(enc3, 0, result, 16, 7);

        return result;
    }

        public static sbyte[] passwordDecodeSBytes(sbyte[] passInput, sbyte[] keyInput)
        {
            int keyValue = BitConverter.ToInt32(ToByteArray(keyInput),0);
        //int keyValue = Convert.ToInt32(keyInput);
            sbyte[] key = new sbyte[4];
            key[0] = (sbyte)keyValue;
            key[1] = (sbyte)(keyValue + 1);
            key[2] = (sbyte)(keyValue + 2);
            key[3] = (sbyte)(keyValue + 3);
        //var key = keyInput;
            sbyte[] dec1 = DecSbytes(passInput, key);
            sbyte[] dec2 = DecSbytes(SlicedSbytes(passInput, 8, 16), key);
            var result = new sbyte[16];
            Buffer.BlockCopy(dec1, 0, result, 0, 8);
            Buffer.BlockCopy(dec2, 0, result, 8, 8);
            return cutTail(result);
        }

        public static sbyte[] DecSbytes(sbyte[] src, sbyte[] key)
        {
            var tempBytev8 = new byte[8];
            Buffer.BlockCopy(src, 0, tempBytev8, 0, 8);
            var v0 = BitConverter.ToUInt32(SlicedBytes(tempBytev8, 0, 4), 0);
            var v1 = BitConverter.ToUInt32(SlicedBytes(tempBytev8, 4, 8), 0);
            var delta = 0x9e3779b9;
            var sum = 0xc6ef3720;
            var tempByteKey = new byte[4];
            Buffer.BlockCopy(key, 0, tempByteKey, 0, 4);
            for (var i = 0; i < 32; i++)
            {
                v1 -= ((v0 << 4) + tempByteKey[2]) ^ (v0 + sum) ^ ((v0 >> 5) + tempByteKey[3]);
                v0 -= ((v1 << 4) + tempByteKey[0]) ^ (v1 + sum) ^ ((v1 >> 5) + tempByteKey[1]);
                sum -= delta;
            }
            return uintToSByteArray(v0, v1);
        }

    }

