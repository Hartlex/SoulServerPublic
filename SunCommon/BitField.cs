using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon
{
    /// <summary>
    /// Specifies the number of bits in the bit field structure
    /// Maximum number of bits are 64
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class BitFieldNumberOfBitsAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of BitFieldNumberOfBitsAttribute with the specified number of bits
        /// </summary>
        /// <param name="bitCount">The number of bits the bit field will contain (Max 64)</param>
        public BitFieldNumberOfBitsAttribute(byte bitCount)
        {
            if ((bitCount < 1) || (bitCount > 64))
                throw new ArgumentOutOfRangeException("bitCount", bitCount,
                    "The number of bits must be between 1 and 64.");

            BitCount = bitCount;
        }

        /// <summary>
        /// The number of bits the bit field will contain
        /// </summary>
        public byte BitCount { get; private set; }
    }
    /// <summary>
    /// Specifies the length of each bit field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BitFieldInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of BitFieldInfoAttribute with the specified field offset and length
        /// </summary>
        /// <param name="offset">The offset of the bit field</param>
        /// <param name="length">The number of bits the bit field occupies</param>
        public BitFieldInfoAttribute(byte offset, byte length)
        {
            Offset = offset;
            Length = length;
        }

        /// <summary>
        /// The offset of the bit field
        /// </summary>
        public byte Offset { get; private set; }

        /// <summary>
        /// The number of bits the bit field occupies
        /// </summary>
        public byte Length { get; private set; }
    }


    /// <summary>
    /// Interface used as a marker in order to create extension methods on a struct
    /// that is used to emulate bit fields
    /// </summary>
    public interface IBitField
    {

    }
    [BitFieldNumberOfBitsAttribute(16)]


    public static class BitfieldHelper
    {
        /// <summary>
        /// Converts the members of the bit field to an integer value.
        /// </summary>
        /// <param name="obj">An instance of a struct that implements the interface IBitField.</param>
        /// <returns>An integer representation of the bit field.</returns>
        public static ulong ToUInt64(this IBitField obj)
        {
            ulong result = 0;

            // Loop through all the properties
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                // Check if the property has an attribute of type BitFieldLengthAttribute
                BitFieldInfoAttribute bitField;
                bitField = (pi.GetCustomAttribute(typeof(BitFieldInfoAttribute)) as BitFieldInfoAttribute);
                if (bitField != null)
                {
                    // Calculate a bitmask using the length of the bit field
                    ulong mask = 0;
                    for (byte i = 0; i < bitField.Length; i++)
                        mask |= 1UL << i;

                    // This conversion makes it possible to use different types in the bit field
                    ulong value = Convert.ToUInt64(pi.GetValue(obj));

                    result |= (value & mask) << bitField.Offset;
                }
            }

            return result;
        }
        /// <summary>
        /// This method converts the struct into a string of binary values.
        /// The length of the string will be equal to the number of bits in the struct.
        /// The least significant bit will be on the right in the string.
        /// </summary>
        /// <param name="obj">An instance of a struct that implements the interface IBitField.</param>
        /// <returns>A string representing the binary value of tbe bit field.</returns>
        public static string ToBinaryString(this IBitField obj)
        {
            BitFieldNumberOfBitsAttribute bitField;
            bitField = (obj.GetType().GetCustomAttribute(typeof(BitFieldNumberOfBitsAttribute)) as BitFieldNumberOfBitsAttribute);
            if (bitField == null)
                throw new Exception(string.Format(@"The attribute 'BitFieldNumberOfBitsAttribute' has to be 
            added to the struct '{0}'.", obj.GetType().Name));

            StringBuilder sb = new StringBuilder(bitField.BitCount);

            ulong bitFieldValue = obj.ToUInt64();
            for (int i = bitField.BitCount - 1; i >= 0; i--)
            {
                sb.Append(((bitFieldValue & (1UL << i)) > 0) ? "1" : "0");
            }

            return sb.ToString();
        }
    }
}
