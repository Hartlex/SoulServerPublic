using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;

namespace MasterServer.Database
{
    internal static class DatabaseFunctions
    {
        public static bool UserLogin(string username, string pw, out int userID)
        
{
            userID = 0;
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select UserID FROM dbo.[User] WHERE [AccName] ='" + username + "' AND [AccPW] ='" + pw + "'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userID = reader.GetInt32(0);
                    return true;
                }

            }

            return false;
        }

        public static bool getAllCharacters(int userID, out List<PacketStructs.CharacterInfo> characterInfos)
        {
            characterInfos = new List<PacketStructs.CharacterInfo>();
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DatabaseHelper.SelectString("*", "Character", "UserID", userID.ToString()) + "AND [DeleteCheck]=0", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var slot = reader.GetByte(2);
                    var size = 0x10;
                    var classCode = (byte)reader.GetByte(3);
                    var charName = reader.GetString(4);
                    var heightCode = reader.GetByte(5);
                    var faceCode = reader.GetByte(6);
                    var hairCode = reader.GetByte(7);
                    var level = ByteUtils.ToByteArray(reader.GetInt16(8), 2);
                    var region = ByteUtils.ToByteArray(reader.GetInt32(28), 4);
                    var locX = ByteUtils.ToByteArray(reader.GetInt16(29), 2);
                    var locY = ByteUtils.ToByteArray(reader.GetInt16(30), 2);
                    var locZ = ByteUtils.ToByteArray(reader.GetInt16(31), 2);
                    var equipnumber = (byte)0;
                    var equipInfo = (byte[])reader[38];
                    if (equipInfo == null || equipInfo.Length == 0) equipInfo = new byte[] { 00 };
                    var charinfo = new PacketStructs.CharacterInfo(slot, size, charName, heightCode, faceCode, hairCode,
                        classCode, level, region, locX, locY, locZ, equipnumber, equipInfo);
                    characterInfos.Add(charinfo);
                }
            }

            return true;
        }
        public static bool GetFullCharacter(int userId, int charSlot, out byte[] fullCharBytes)
        {
            fullCharBytes = null;
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM dbo.[Character] WHERE [UserID]=" + userId + " AND [charSlot]=" + charSlot, conn);
                var reader = cmd.ExecuteReader();
                var result = new List<byte>();
                while (reader.Read())
                {
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(0)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(1)));
                    result.Add(reader.GetByte(2));
                    result.Add(reader.GetByte(3));

                    result.AddRange(ByteUtils.ToByteArray(reader.GetString(4), 16));
                    //result.AddRange(Encoding.ASCII.GetBytes(reader.GetString(3)));
                    result.Add(reader.GetByte(5));
                    result.Add(reader.GetByte(6));
                    result.Add(reader.GetByte(7));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt16(8)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(9)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(10)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(11)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(12)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(13)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(14)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(15)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(16)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt64(17)));
                    result.AddRange(BitConverter.GetBytes(reader.GetFloat(18)));
                    result.AddRange(BitConverter.GetBytes(reader.GetFloat(19)));
                    result.AddRange(BitConverter.GetBytes(reader.GetFloat(20)));
                    result.AddRange(BitConverter.GetBytes(reader.GetFloat(21)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt64(22)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(23)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(24)));
                    result.Add(reader.GetByte(25));
                    result.Add(reader.GetByte(26));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt64(27)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(28)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt16(29)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt16(30)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt16(31)));
                    result.AddRange(ByteUtils.ToByteArray(reader[32].ToString(), 16));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt64(33)));
                    result.Add(reader.GetByte(34));
                    result.Add(reader.GetByte(35));
                    var bytes = (byte[])reader[36];
                    result.Add((byte)bytes.Length);
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[35]);
                    bytes = (byte[])reader[37];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[36]);
                    bytes = (byte[])reader[38];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[37]);
                    bytes = (byte[])reader[39];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[38]);
                    bytes = (byte[])reader[40];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[39]);
                    bytes = (byte[])reader[41];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[40]);
                    bytes = (byte[])reader[42];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[41]);
                    bytes = (byte[])reader[43];
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
                    result.AddRange(bytes);
                    //result.AddRange((byte[])reader[42]);
                    result.AddRange(BitConverter.GetBytes(reader.GetInt64(44)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(45)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(46)));
                    result.Add(reader.GetByte(47));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(48)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(49)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(50)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(51)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(52)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(53)));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(54)));
                    result.Add(reader.GetByte(55));
                    result.AddRange(BitConverter.GetBytes(reader.GetInt32(56)));
                    result.AddRange(ByteUtils.ToByteArray(reader[57].ToString(), 16));
                    fullCharBytes = result.ToArray();
                    return true;
                }
            }

            return false;
        }
    }
}
