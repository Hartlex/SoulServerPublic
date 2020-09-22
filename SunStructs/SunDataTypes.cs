using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunStructs
{
    public class SunDataTypes
    {
        public struct SunVector
        {
            private short x;
            private short y;
            private short z;

            public SunVector(short x, short y, short z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public SunVector(byte[] bytes)
            {
                this.x = BitConverter.ToInt16(new byte[] {bytes[0], bytes[1]}, 0);
                this.y = BitConverter.ToInt16(new byte[] { bytes[2], bytes[3] }, 0);
                this.z = BitConverter.ToInt16(new byte[] { bytes[4], bytes[5] }, 0);
            }
            
            public byte[] GetBytes()
            {
                var result = new byte[6];
                Buffer.BlockCopy(BitConverter.GetBytes(x),0,result,0,2);
                Buffer.BlockCopy(BitConverter.GetBytes(y), 0, result, 2, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(z), 0, result, 4, 2);
                return result;
            }
        }



    }
}
