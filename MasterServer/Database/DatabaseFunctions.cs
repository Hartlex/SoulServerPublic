using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;
using SunCommon.Entities;

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
                    result.AddRange(BitConverter.GetBytes(bytes.Length));
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
        public static bool CreateCharacter(int UserID, string charName, byte classCode, byte heightCode, byte faceCode,
            byte hairCode, out Character character)
        {
            character = null;
            var slot = FindFreeCharacterSlot(UserID);
            if (slot == -1) return false;
            CharacterSetDBEntity charSet = new CharacterSetDBEntity(classCode);
            character = DatabaseHelper.CreateCharacter(slot, UserID, charName, heightCode, faceCode, hairCode, charSet);

            return true;

        }
        public static int FindFreeCharacterSlot(int userID)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DatabaseHelper.SelectString("charSlot", "Character", "UserID", userID.ToString()) + "AND [DeleteCheck]=0", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                var charSlotList = new List<int>();
                while (reader.Read())
                {
                    charSlotList.Add(reader.GetByte(0));
                }

                for (int i = 0; i < 6; i++)
                {
                    if (!charSlotList.Contains(i)) return i;
                }

                return -1;

            }
        }
        public static bool AddCharacterToDB(Character c, out int charID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    conn.Open();
                    #region sqlstring
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO [Character] " +
                        DatabaseHelper.CharacterTableColumns() +
                        " VALUES (" +
                        "@userID," +
                        "@charSlot," +
                        "@ClassCode," +
                        "@CharName," +
                        "@HeightCode," +
                        "@FaceCode," +
                        "@HairCode," +
                        "@Level," +
                        "@Strength," +
                        "@Dexterity," +
                        "@Vitality," +
                        "@Intelligence," +
                        "@Spirit," +
                        "@SkillStat1," +
                        "@SkilStat2," +
                        "@UserPoint," +
                        "@Experience," +
                        "@MaxHp," +
                        "@Hp," +
                        "@MaxMp," +
                        "@Mp," +
                        "@Money," +
                        "@RemainStat," +
                        "@RemainSkill," +
                        "@PkState," +
                        "@CharState," +
                        "@StateTime," +
                        "@Region," +
                        "@LocationX," +
                        "@LocationY," +
                        "@LocationZ," +
                        //"@TitleID," +
                        "@TitleTime," +
                        "@InvisOpt," +
                        "@InventoryLock," +
                        "@InventoryItem," +
                        "@TmpInvenoryItem," +
                        "@EquipItem," +
                        "@Skill," +
                        "@QuickSkill," +
                        "@Style," +
                        "@Quest," +
                        "@Mission," +
                        "@PlayLimitedTime," +
                        "@PVPPoint," +
                        "@PVPScore," +
                        "@PVPGrade," +
                        "@PVPDraw," +
                        "@PVPSeries," +
                        "@PVPKill," +
                        "@PVPDeath," +
                        "@PVPMaxKill," +
                        "@PVPMaxDeath," +
                        "@GuildID," +
                        "@GuildPosition," +
                        "@GuildUserPoint," +
                        //"@GuildNickName," +
                        "@CreatonDate," +
                        "@ModifiedDate," +
                        "@LastDate," +
                        "@DeleteCheck)" +
                        "select scope_identity()" +
                        "", conn);

                    #endregion
                    #region params

                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = c.Account.Id;
                    cmd.Parameters.Add("@charSlot", SqlDbType.Int).Value = c.Slot;
                    cmd.Parameters.Add("@ClassCode", SqlDbType.Int).Value = c.ClassCode;
                    cmd.Parameters.Add("@CharName", SqlDbType.VarChar).Value = c.CharName;
                    cmd.Parameters.Add("@HeightCode", SqlDbType.Int).Value = c.HeightCode;
                    cmd.Parameters.Add("@FaceCode", SqlDbType.Int).Value = c.FaceCode;
                    cmd.Parameters.Add("@HairCode", SqlDbType.Int).Value = c.HairCode;
                    cmd.Parameters.Add("@Level", SqlDbType.SmallInt).Value = c.Level;
                    cmd.Parameters.Add("@Strength", SqlDbType.Int).Value = c.Strength;
                    cmd.Parameters.Add("@Dexterity", SqlDbType.Int).Value = c.Dexterity;

                    cmd.Parameters.Add("@Vitality", SqlDbType.Int).Value = c.Vitality;
                    cmd.Parameters.Add("@Intelligence", SqlDbType.Int).Value = c.Intelligence;
                    cmd.Parameters.Add("@Spirit", SqlDbType.Int).Value = c.Spirit;
                    cmd.Parameters.Add("@SkillStat1", SqlDbType.Int).Value = c.SkillStat1;
                    cmd.Parameters.Add("@SkilStat2", SqlDbType.Int).Value = c.SkillStat2;
                    cmd.Parameters.Add("@UserPoint", SqlDbType.Int).Value = c.UserPoint;
                    cmd.Parameters.Add("@Experience", SqlDbType.BigInt).Value = c.Experience;
                    cmd.Parameters.Add("@MaxHp", SqlDbType.Real).Value = c.MaxHp;
                    cmd.Parameters.Add("@Hp", SqlDbType.Real).Value = c.Hp;
                    cmd.Parameters.Add("@MaxMp", SqlDbType.Real).Value = c.MaxMp;

                    cmd.Parameters.Add("@Mp", SqlDbType.Real).Value = c.Mp;
                    cmd.Parameters.Add("@Money", SqlDbType.BigInt).Value = c.Inventory.Money;
                    cmd.Parameters.Add("@RemainStat", SqlDbType.Int).Value = c.RemainStat;
                    cmd.Parameters.Add("@RemainSkill", SqlDbType.Int).Value = c.RemainSkill;
                    cmd.Parameters.Add("@PkState", SqlDbType.TinyInt).Value = c.PkState;
                    cmd.Parameters.Add("@CharState", SqlDbType.TinyInt).Value = c.CharState;
                    cmd.Parameters.Add("@StateTime", SqlDbType.TinyInt).Value = c.StateTime;
                    cmd.Parameters.Add("@Region", SqlDbType.Int).Value = c.CharacterPosition.Region;
                    cmd.Parameters.Add("@LocationX", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationX;
                    cmd.Parameters.Add("@LocationY", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationY;

                    cmd.Parameters.Add("@LocationZ", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationZ;
                    cmd.Parameters.Add("@TitleTime", SqlDbType.BigInt).Value = c.TitleTime;
                    cmd.Parameters.Add("@InvisOpt", SqlDbType.TinyInt).Value = c.InvisibleOpt;
                    cmd.Parameters.Add("@InventoryLock", SqlDbType.TinyInt).Value = c.Inventory.InventoryLock;
                    cmd.Parameters.Add("@InventoryItem", SqlDbType.VarBinary).Value = c.Inventory.InventoryItem;
                    cmd.Parameters.Add("@TmpInvenoryItem", SqlDbType.VarBinary).Value = c.Inventory.TmpInventoryItem;
                    cmd.Parameters.Add("@EquipItem", SqlDbType.VarBinary).Value = c.Inventory.EquipItem;
                    cmd.Parameters.Add("@Skill", SqlDbType.VarBinary).Value = c.Skill;
                    cmd.Parameters.Add("@QuickSkill", SqlDbType.VarBinary).Value = c.Quick;
                    cmd.Parameters.Add("@Style", SqlDbType.VarBinary).Value = c.Style;

                    cmd.Parameters.Add("@Quest", SqlDbType.VarBinary).Value = c.Quest;
                    cmd.Parameters.Add("@Mission", SqlDbType.VarBinary).Value = c.Mission;
                    cmd.Parameters.Add("@PlayLimitedTime", SqlDbType.BigInt).Value = c.PlayLimitedTime;
                    cmd.Parameters.Add("@PVPPoint", SqlDbType.Int).Value = c.PvpPoint;
                    cmd.Parameters.Add("@PVPScore", SqlDbType.Int).Value = c.PvpScore;
                    cmd.Parameters.Add("@PVPGrade", SqlDbType.TinyInt).Value = c.PvpGrade;
                    cmd.Parameters.Add("@PVPDraw", SqlDbType.Int).Value = c.PvpDraw;
                    cmd.Parameters.Add("@PVPSeries", SqlDbType.Int).Value = c.PvpSeries;
                    cmd.Parameters.Add("@PVPKill", SqlDbType.Int).Value = c.PvpKill;
                    cmd.Parameters.Add("@PVPDeath", SqlDbType.Int).Value = c.PvpDie;

                    cmd.Parameters.Add("@PVPMaxKill", SqlDbType.Int).Value = c.PvpMaxKill;
                    cmd.Parameters.Add("@PVPMaxDeath", SqlDbType.Int).Value = c.PvpMaxDie;
                    cmd.Parameters.Add("@GuildID", SqlDbType.Int).Value = c.GuildID;
                    cmd.Parameters.Add("@GuildPosition", SqlDbType.TinyInt).Value = c.GuildPosition;
                    cmd.Parameters.Add("@GuildUserPoint", SqlDbType.Int).Value = c.GuildUserPoint;
                    cmd.Parameters.Add("@CreatonDate", SqlDbType.SmallDateTime).Value = c.CreationDate;
                    cmd.Parameters.Add("@ModifiedDate", SqlDbType.SmallDateTime).Value = c.ModifiedDate;
                    cmd.Parameters.Add("@LastDate", SqlDbType.SmallDateTime).Value = c.LastLoginDate;
                    cmd.Parameters.Add("@DeleteCheck", SqlDbType.TinyInt).Value = c.DeleteCheck;
                    #endregion
                    charID = Convert.ToInt32(cmd.ExecuteScalar());

                }

                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                charID = -1;
                return false;
            }
        }
        public static bool DeleteCharacter(int userId, int charSlot)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT [charID] FROM dbo.[Character] WHERE [UserID] =" + userId + " AND [charSlot]= " + charSlot, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var charid = reader.GetInt32(0);
                    reader.Close();
                    SqlCommand cmd2 = new SqlCommand("UPDATE dbo.[Character] SET [DeleteCheck]=1 WHERE [charID]=" + charid, conn);
                    var reader2 = cmd2.ExecuteReader();
                    if (reader2.RecordsAffected > 0)
                        return true;
                }

                return false;
            }
        }
        public static bool IsNameFree(string name)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT [CharName] FROM dbo.[Character] WHERE [CharName]= '" + name + "'", conn);
                var reader = cmd.ExecuteReader();
                return !reader.HasRows;
            }
        }

        public static bool UpdateFullCharacter(Character c)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE dbo.[Character] SET " +
                        "[Level] = @Level," +
                        "[Strength] = @Strength," +
                        "[Dexterity]= @Dexterity," +
                        "[Vitality]= @Vitality," +
                        "[Intelligence] = @Intelligence," +
                        "[Spirit] = @Spirit," +
                        "[SkillStat1] = @SkillStat1," +
                        "[SkillStat2] = @SkillStat2," +
                        "[UserPoint] =@UserPoint," +
                        "[Experience] =@Experience," +
                        "[MaxHp] = @MaxHp," +
                        "[Hp] = @Hp," +
                        "[MaxMp]=@MaxMp," +
                        "[Mp]=@Mp," +
                        "[Money] = @Money," +
                        "[RemainStat] = @RemainStat," +
                        "[RemainSkill] = @RemainSkill," +
                        "[PkState] =@PkState," +
                        "[CharState] =@CharState," +
                        "[StateTime] =@StateTime," +
                        "[Region] =@Region," +
                        "[LocationX] =@LocationX," +
                        "[LocationY] = @LocationY," +
                        "[LocationZ] = @LocationZ," +
                        "[TitleID] = @TitleID," +
                        "[TitleTime] =@TitleTime," +
                        "[InvisOpt] =@InvisOpt," +
                        "[InventoryLock] = @InventoryLock," +
                        "[InventoryItem] = @InventoryItem," +
                        "[TmpInventoryItem] =@TmpInventoryItem," +
                        "[EquipItem] = @EquipItem," +
                        "[Skill] = @Skill," +
                        "[QuickSkill] = @QuickSkill," +
                        "[Style] = @Style," +
                        "[Quest] = @Quest," +
                        "[Mission] =@Mission," +
                        "[PlayLimitedTime] =@PlayLimitiedTime," +
                        "[PVPPoint] = @PVPPoint," +
                        "[PVPScore]=@PVPScore," +
                        "[PVPGrade] =@PVPGrade," +
                        "[PVPDraw] =@PVPDraw," +
                        "[PVPSeries] =@PVPSeries," +
                        "[PVPKill] =@PVPKill," +
                        "[PVPDeath] =@PVPDeath," +
                        "[PVPMaxKill] =@PVPMaxKill," +
                        "[PVPMaxDeath] =@PVPMaxDeath," +
                        "[GuildID] = @GuildId," +
                        "[GuildPosition] =@GuildPosition," +
                        "[GuildUserPoint] =@GuildUserPoint," +
                        "[GuildNickName]=@GuildNickName," +
                        "[CreationDate]=@CreationDate," +
                        "[ModifiedDate]=@ModifiedDate," +
                        "[LastDate]=@LastDate," +
                        "[DeleteCheck] =@DeleteCheck" +
                        "WHERE [charID]=@charID",conn
                        );
                    #region params

                    cmd.Parameters.Add("@charId", SqlDbType.Int).Value = c.Id;
                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value = c.Account.Id;
                    cmd.Parameters.Add("@charSlot", SqlDbType.Int).Value = c.Slot;
                    cmd.Parameters.Add("@ClassCode", SqlDbType.Int).Value = c.ClassCode;
                    cmd.Parameters.Add("@CharName", SqlDbType.VarChar).Value = c.CharName;
                    cmd.Parameters.Add("@HeightCode", SqlDbType.Int).Value = c.HeightCode;
                    cmd.Parameters.Add("@FaceCode", SqlDbType.Int).Value = c.FaceCode;
                    cmd.Parameters.Add("@HairCode", SqlDbType.Int).Value = c.HairCode;
                    cmd.Parameters.Add("@Level", SqlDbType.SmallInt).Value = c.Level;
                    cmd.Parameters.Add("@Strength", SqlDbType.Int).Value = c.Strength;
                    cmd.Parameters.Add("@Dexterity", SqlDbType.Int).Value = c.Dexterity;

                    cmd.Parameters.Add("@Vitality", SqlDbType.Int).Value = c.Vitality;
                    cmd.Parameters.Add("@Intelligence", SqlDbType.Int).Value = c.Intelligence;
                    cmd.Parameters.Add("@Spirit", SqlDbType.Int).Value = c.Spirit;
                    cmd.Parameters.Add("@SkillStat1", SqlDbType.Int).Value = c.SkillStat1;
                    cmd.Parameters.Add("@SkilStat2", SqlDbType.Int).Value = c.SkillStat2;
                    cmd.Parameters.Add("@UserPoint", SqlDbType.Int).Value = c.UserPoint;
                    cmd.Parameters.Add("@Experience", SqlDbType.BigInt).Value = c.Experience;
                    cmd.Parameters.Add("@MaxHp", SqlDbType.Real).Value = c.MaxHp;
                    cmd.Parameters.Add("@Hp", SqlDbType.Real).Value = c.Hp;
                    cmd.Parameters.Add("@MaxMp", SqlDbType.Real).Value = c.MaxMp;

                    cmd.Parameters.Add("@Mp", SqlDbType.Real).Value = c.Mp;
                    cmd.Parameters.Add("@Money", SqlDbType.BigInt).Value = c.Inventory.Money;
                    cmd.Parameters.Add("@RemainStat", SqlDbType.Int).Value = c.RemainStat;
                    cmd.Parameters.Add("@RemainSkill", SqlDbType.Int).Value = c.RemainSkill;
                    cmd.Parameters.Add("@PkState", SqlDbType.TinyInt).Value = c.PkState;
                    cmd.Parameters.Add("@CharState", SqlDbType.TinyInt).Value = c.CharState;
                    cmd.Parameters.Add("@StateTime", SqlDbType.TinyInt).Value = c.StateTime;
                    cmd.Parameters.Add("@Region", SqlDbType.Int).Value = c.CharacterPosition.Region;
                    cmd.Parameters.Add("@LocationX", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationX;
                    cmd.Parameters.Add("@LocationY", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationY;

                    cmd.Parameters.Add("@LocationZ", SqlDbType.SmallInt).Value = (short)c.CharacterPosition.LocationZ;
                    cmd.Parameters.Add("@TitleTime", SqlDbType.BigInt).Value = c.TitleTime;
                    cmd.Parameters.Add("@InvisOpt", SqlDbType.TinyInt).Value = c.InvisibleOpt;
                    cmd.Parameters.Add("@InventoryLock", SqlDbType.TinyInt).Value = c.Inventory.InventoryLock;
                    cmd.Parameters.Add("@InventoryItem", SqlDbType.VarBinary).Value = c.Inventory.InventoryItem;
                    cmd.Parameters.Add("@TmpInvenoryItem", SqlDbType.VarBinary).Value = c.Inventory.TmpInventoryItem;
                    cmd.Parameters.Add("@EquipItem", SqlDbType.VarBinary).Value = c.Inventory.EquipItem;
                    cmd.Parameters.Add("@Skill", SqlDbType.VarBinary).Value = c.Skill;
                    cmd.Parameters.Add("@QuickSkill", SqlDbType.VarBinary).Value = c.Quick;
                    cmd.Parameters.Add("@Style", SqlDbType.VarBinary).Value = c.Style;

                    cmd.Parameters.Add("@Quest", SqlDbType.VarBinary).Value = c.Quest;
                    cmd.Parameters.Add("@Mission", SqlDbType.VarBinary).Value = c.Mission;
                    cmd.Parameters.Add("@PlayLimitedTime", SqlDbType.BigInt).Value = c.PlayLimitedTime;
                    cmd.Parameters.Add("@PVPPoint", SqlDbType.Int).Value = c.PvpPoint;
                    cmd.Parameters.Add("@PVPScore", SqlDbType.Int).Value = c.PvpScore;
                    cmd.Parameters.Add("@PVPGrade", SqlDbType.TinyInt).Value = c.PvpGrade;
                    cmd.Parameters.Add("@PVPDraw", SqlDbType.Int).Value = c.PvpDraw;
                    cmd.Parameters.Add("@PVPSeries", SqlDbType.Int).Value = c.PvpSeries;
                    cmd.Parameters.Add("@PVPKill", SqlDbType.Int).Value = c.PvpKill;
                    cmd.Parameters.Add("@PVPDeath", SqlDbType.Int).Value = c.PvpDie;

                    cmd.Parameters.Add("@PVPMaxKill", SqlDbType.Int).Value = c.PvpMaxKill;
                    cmd.Parameters.Add("@PVPMaxDeath", SqlDbType.Int).Value = c.PvpMaxDie;
                    cmd.Parameters.Add("@GuildID", SqlDbType.Int).Value = c.GuildID;
                    cmd.Parameters.Add("@GuildPosition", SqlDbType.TinyInt).Value = c.GuildPosition;
                    cmd.Parameters.Add("@GuildUserPoint", SqlDbType.Int).Value = c.GuildUserPoint;
                    cmd.Parameters.Add("@CreatonDate", SqlDbType.SmallDateTime).Value = c.CreationDate;
                    cmd.Parameters.Add("@ModifiedDate", SqlDbType.SmallDateTime).Value = c.ModifiedDate;
                    cmd.Parameters.Add("@LastDate", SqlDbType.SmallDateTime).Value = c.LastLoginDate;
                    cmd.Parameters.Add("@DeleteCheck", SqlDbType.TinyInt).Value = c.DeleteCheck;
                    #endregion

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                    }

                    return true;
                }
            }
            catch (SqlException e)
            {

            }

            return false;
        }
    }
}
