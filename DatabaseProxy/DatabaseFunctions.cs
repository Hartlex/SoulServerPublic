using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunCommon;
using SunCommon.Entities;
using static DatabaseProxy.DatabaseHelper;

namespace DatabaseProxy
{
    static class DatabaseFunctions
    {
        public static bool UserLogin(string username,string pw, out int userID)
        {
            userID = 0;
            using (SqlConnection conn = new SqlConnection(GetSqlConnectionString().ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select UserID FROM dbo.[User] WHERE [AccName] ='"+username+"' AND [AccPW] ='"+pw+"'" , conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userID = reader.GetInt32(0);
                    return true;
                }

            }

            return false;
        }

        public static int FindFreeCharacterSlot(int userID)
        {
            using (SqlConnection conn = DbConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SelectString("charSlot", "Character", "UserID", userID.ToString()),conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    return reader.GetInt32(0);
                }
                if (reader.RecordsAffected == -1) return 0; //First Character
                if (reader.RecordsAffected >= 8) return -1; //Max Characters
            }
            return -1;
        }

        public static bool CreateCharacter(int UserID, string charName, int classCode, int heightCode, int faceCode,
            int hairCode, out Character character)
        {
            character = null;
            var slot = FindFreeCharacterSlot(UserID);
            if (slot == -1) return false;
            CharacterSetDBEntity charSet = new CharacterSetDBEntity(classCode);
            character = DatabaseHelper.CreateCharacter(slot,UserID, charName, heightCode, faceCode, hairCode,charSet);

            return true;

        }

        public static bool AddCharacterToDB(Character c,out int charID)
        {
            try
            {
                using (SqlConnection conn = DbConnection())
                {
                    conn.Open();
                    #region sqlstring
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO [Character] " +
                        CharacterTableColumns() +
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
                        "@Intelligence,"+
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
                        "",conn);

                    #endregion
                    #region params

                    cmd.Parameters.Add("@userID", SqlDbType.Int).Value=c.Account.Id;
                    cmd.Parameters.Add("@charSlot", SqlDbType.Int).Value = c.Slot;
                    cmd.Parameters.Add("@ClassCode", SqlDbType.Int).Value = c.ClassCode;
                    cmd.Parameters.Add("@CharName", SqlDbType.VarChar).Value = c.CharName;
                    cmd.Parameters.Add("@HeightCode", SqlDbType.Int).Value = c.HeightCode;
                    cmd.Parameters.Add("@FaceCode", SqlDbType.Int).Value = c.FaceCode;
                    cmd.Parameters.Add("@HairCode", SqlDbType.Int).Value = c.HairCode;
                    cmd.Parameters.Add("@Level", SqlDbType.BigInt).Value = c.Level;
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
                    cmd.Parameters.Add("@Money", SqlDbType.BigInt).Value = c.Money;
                    cmd.Parameters.Add("@RemainStat", SqlDbType.Int).Value = c.RemainStat;
                    cmd.Parameters.Add("@RemainSkill", SqlDbType.Int).Value = c.RemainSkill;
                    cmd.Parameters.Add("@PkState", SqlDbType.TinyInt).Value = c.PkState;
                    cmd.Parameters.Add("@CharState", SqlDbType.TinyInt).Value = c.CharState;
                    cmd.Parameters.Add("@StateTime", SqlDbType.TinyInt).Value = c.StateTime;
                    cmd.Parameters.Add("@Region", SqlDbType.Int).Value = c.CharacterPosition.Region;
                    cmd.Parameters.Add("@LocationX", SqlDbType.SmallInt).Value = c.CharacterPosition.LocationX;
                    cmd.Parameters.Add("@LocationY", SqlDbType.SmallInt).Value = c.CharacterPosition.LocationY;

                    cmd.Parameters.Add("@LocationZ", SqlDbType.SmallInt).Value = c.CharacterPosition.LocationZ;
                    cmd.Parameters.Add("@TitleTime", SqlDbType.BigInt).Value = c.TitleTime;
                    cmd.Parameters.Add("@InvisOpt", SqlDbType.TinyInt).Value = c.InvisibleOpt;
                    cmd.Parameters.Add("@InventoryLock", SqlDbType.TinyInt).Value = c.InventoryLock;
                    cmd.Parameters.Add("@InventoryItem", SqlDbType.VarBinary).Value = c.Inventory.InventoryItem;
                    cmd.Parameters.Add("@TmpInvenoryItem", SqlDbType.VarBinary).Value = c.Inventory.TmpInventoryItem;
                    cmd.Parameters.Add("@EquipItem", SqlDbType.VarBinary).Value = c.Inventory.EquipItem;
                    cmd.Parameters.Add("@Skill", SqlDbType.VarBinary).Value = c.Skill;
                    cmd.Parameters.Add("@QuickSkill", SqlDbType.VarBinary).Value = c.Quick;
                    cmd.Parameters.Add("@Style", SqlDbType.VarBinary).Value = c.Style;

                    cmd.Parameters.Add("@Quest", SqlDbType.VarBinary).Value = c.Quest;
                    cmd.Parameters.Add("@Mission", SqlDbType.VarBinary).Value = c.Mission;
                    cmd.Parameters.Add("@PlayLimitedTime", SqlDbType.BigInt).Value = c.PlayLimitiedTime;
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
                    charID =Convert.ToInt32(cmd.ExecuteScalar());
                    
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

        public static bool getAllCharacters(int userID,out List<PacketStructs.CharacterInfo> characterInfos)
        {
            characterInfos = new List<PacketStructs.CharacterInfo>();
            using (SqlConnection conn= DbConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SelectString("*","Character","UserID",userID.ToString()),conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var slot = reader.GetInt32(2);
                    var size = 0x10;
                    var classCode = (byte)reader.GetInt32(3);
                    var charName = reader.GetString(4);
                    var heightCode = (byte)reader.GetInt32(5);
                    var faceCode = (byte)reader.GetInt32(6);
                    var hairCode = (byte)reader.GetInt32(7);
                    var level = ByteUtils.ToByteArray(reader.GetInt16(8), 2);
                    var region = ByteUtils.ToByteArray(reader.GetInt32(28),4);
                    var locX = ByteUtils.ToByteArray(reader.GetInt16(29),2);
                    var locY = ByteUtils.ToByteArray(reader.GetInt16(30), 2);
                    var locZ = ByteUtils.ToByteArray(reader.GetInt16(31), 2);
                    var equipnumber = (byte)0;
                    var equipInfo = (byte[]) reader[38];
                    //reader.GetBytes(38, 0, equipInfo, 0, 289);
                    var charinfo = new PacketStructs.CharacterInfo(slot, size, charName, heightCode, faceCode, hairCode,
                        classCode, level, region, locX, locY, locZ, equipnumber, equipInfo);
                    characterInfos.Add(charinfo);
                }
            }

            return true;
        }
    }
}
