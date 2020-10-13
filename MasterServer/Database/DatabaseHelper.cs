using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using MasterServer.Properties;
using SunCommon.Entities;

namespace MasterServer.Database
{
    internal static class DatabaseHelper
    {
        private static string _connectionString;

        public static string GetConnectionString()
        {
            return _connectionString;
        }
        public static bool TestDbConnection(out SqlConnection connection)
        {
            string sqlConnectionString = LoadDatabaseConfig();
            Console.WriteLine(Resources.DatabaseHelper_TestDbConnection_Load);
            connection = null;
            try
            {
                connection = new SqlConnection(sqlConnectionString);
                connection.Open();
                connection.Close();
                Console.WriteLine(Resources.DatabaseHelper_TestDbConnection_Success);
                return true;
            }
            catch (SqlException e)
            {
                //TODO to errorLog
                return false;
            }
        }

        private static string LoadDatabaseConfig()
        {
            Console.WriteLine(Resources.DatabaseHelper_LoadDatabaseConfig_Load);
            try
            {
                string dataSource = ConfigurationManager.AppSettings["SQLServer"];
                string userId = ConfigurationManager.AppSettings["DbUserId"];
                string password = ConfigurationManager.AppSettings["DbPassword"];
                string initCatalog = ConfigurationManager.AppSettings["database"];

                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = dataSource,
                    UserID = userId,
                    Password = password,
                    InitialCatalog = initCatalog,
                    IntegratedSecurity = false
                };
                Console.WriteLine(Resources.DatabaseHelper_LoadDatabaseConfig_Success);
                _connectionString = sqlBuilder.ConnectionString;
                return sqlBuilder.ConnectionString;
            }
            catch (Exception e)
            {
                //TODO out to errorLog
                return null;
            }

        }
        public static string SelectString(string SelectColumn, string inTable, string conditionColumn,
            string conditionValue)
        {
            return "SELECT " + SelectColumn + " FROM dbo.[" + inTable + "] WHERE [" + conditionColumn + "]=" +
                   conditionValue;
        }
        public static string SelectString(string table, Dictionary<string,string> conditionDictionary, params string[] selectColumns)
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            if (selectColumns.Length == 1)
            {
                if (selectColumns[0] == "*") sb.Append(selectColumns[0]);
                else sb.Append("[" + selectColumns[0] + "] ");
            }
            else
                foreach (var column in selectColumns)
                {
                    if(column == selectColumns.Last()) sb.Append("[" + column + "]");
                    else sb.Append("[" + column + "],");
                }

            sb.Append("FROM dbo.[" + table + "] WHERE ");
            foreach (var condition in conditionDictionary)
            {
                if(condition.Equals(conditionDictionary.Last()))
                    sb.Append("[" + condition.Key + "] = " + condition.Value);
                else
                    sb.Append("[" + condition.Key + "] = " + condition.Value+" AND ");
            }

            return sb.ToString();
        }

        public static string SelectString(string table)
        {
            return "SELECT * FROM dbo.[" + table + "]";
        }

        public static string SelectString(string table, string conditionColumn, string conditionValue)
        {
            return "SELECT * FROM dbo.[ " + table + "] WHERE [" + conditionColumn + "] =" + conditionValue;
        }
        public static Character CreateCharacter(int slot, int userId, string charName, byte heightCode, byte faceCode, byte hairCode, CharacterSetDBEntity set)
        {
            Character c = new Character();
            var inv = new Inventory();
            c.Inventory = inv;
            c.Account = new Account(userId);
            c.Slot = slot;
            c.ClassCode = set.classCode;
            c.CharName = charName;
            c.HeightCode = heightCode;
            c.FaceCode = faceCode;
            c.HairCode = hairCode;
            c.Level = set.level;
            c.UserPoint = set.UserPoint;
            c.MaxHp = set.MaxHp;
            c.Hp = set.MaxHp;
            c.MaxMp = set.MaxMp;
            c.Mp = set.MaxMp;
            c.Inventory.Money = set.money;
            c.Experience = set.experience;
            c.PkState = 1;
            c.CharState = 1;
            c.StateTime = 1;
            c.CharacterPosition = new CharacterPosition
            {
                Region = set.region,
                LocationX = (float)set.LocationX,
                LocationZ = (float)set.LocationZ,
                LocationY = (float)set.LocationY,
                Angle = 0
            };
            c.TitleID = null;
            c.TitleTime = 1;
            c.InvisibleOpt = 0;
            c.Inventory.InventoryLock = 0;
            c.Inventory = new Inventory
            {
                InventoryItem = set.inventoryItem,
                TmpInventoryItem = set.tmpInventoryItem,
                EquipItem = set.equipItem
            };
            c.Skill = set.skill;
            c.Quick = set.quick;
            c.Style = set.style;
            c.Quest = set.quest;
            c.Mission = set.mission;
            c.RemainSkill = set.RemainSkill;
            c.RemainStat = set.RemainStat;
            c.SelectedStyle = set.selectedStyle;
            c.Strength = set.strength;
            c.Dexterity = set.dexterity;
            c.Vitality = set.vitality;
            c.Intelligence = set.intelligence;
            c.Spirit = set.spirit;
            c.SkillStat1 = set.skillStat1;
            c.SkillStat2 = set.skillStat2;
            c.PlayLimitedTime = 0;
            c.GuildNickName = null;
            c.CreationDate = DateTime.Now;
            c.ModifiedDate = DateTime.Now;
            c.LastLoginDate = DateTime.Now;
            c.DeleteCheck = 0;
            return c;
        }

        public static string CharacterTableColumns()
        {
            return "(" +
                      "UserID," +
                      "charSlot," +
                      "ClassCode," +
                      "CharName," +
                      "HeightCode," +
                      "FaceCode," +
                      "HairCode," +
                      "Level," +
                      "Strength," +
                      "Dexterity," +
                      "Vitality," +
                      "Intelligence," +
                      "Spirit," +
                      "SkillStat1," +
                      "SkilStat2," +
                      "UserPoint," +
                      "Experience," +
                      "MaxHp," +
                      "Hp," +
                      "MaxMp," +
                      "Mp," +
                      "Money," +
                      "RemainStat," +
                      "RemainSkill," +
                      "PkState," +
                      "CharState," +
                      "StateTime," +
                      "Region," +
                      "LocationX," +
                      "LocationY," +
                      "LocationZ," +
                      //"TitleID," +
                      "TitleTime," +
                      "InvisOpt," +
                      "InventoryLock," +
                      "InventoryItem," +
                      "TmpInventoryItem," +
                      "EquipItem," +
                      "Skill," +
                      "QuickSkill," +
                      "Style," +
                      "Quest," +
                      "Mission," +
                      "PlayLimitedTime," +
                      "PVPPoint," +
                      "PVPScore," +
                      "PVPGrade," +
                      "PVPDraw," +
                      "PVPSeries," +
                      "PVPKill," +
                      "PVPDeath," +
                      "PVPMaxKill," +
                      "PVPMaxDeath," +
                      "GuildID," +
                      "GuildPosition," +
                      "GuildUserPoint," +
                      //"GuildNickName," +
                      "CreatonDate," +
                      "ModifiedDate," +
                      "LastDate," +
                      "DeleteCheck)";
        }
        //public static string SelectString(string table, string column, string conditionColumn, string conditionValue)
        //{
        //    return "SELECT ["+column+"] FROM dbo.[ " + table + "] WHERE [" + conditionColumn + "] =" + conditionValue;
        //}
    }
    public class CharacterSetDBEntity
    {
        public byte charCode, classCode;
        public string className;
        public short level;
        public int UserPoint;
        public Single MaxHp, MaxMp;
        public long money;
        public int experience;
        public int RemainStat, RemainSkill, selectedStyle, region;
        public short LocationX, LocationY, LocationZ;
        public short strength, dexterity, vitality, intelligence, spirit, skillStat1, skillStat2;
        public byte[] inventoryItem = { 00 };
        public byte[] tmpInventoryItem = { 00 };
        public byte[] equipItem = { 00 };
        public byte[] skill = { 00 };
        public byte[] quick = { 00 };
        public byte[] style = { 00 };
        public byte[] quest = { 00 };
        public byte[] mission = { 00 };

        public CharacterSetDBEntity(int classCode)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DatabaseHelper.SelectString("*", "CharacterSet", "ClassCode", classCode.ToString()), conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    charCode = reader.GetByte(0);
                    this.classCode = reader.GetByte(1);
                    className = reader.GetString(2);
                    level = reader.GetInt16(3);
                    UserPoint = reader.GetInt32(4);
                    MaxHp = reader.GetFloat(5);
                    MaxMp = reader.GetFloat(6);
                    money = reader.GetInt64(7);
                    experience = reader.GetInt32(8);
                    RemainStat = reader.GetInt32(9);
                    RemainSkill = reader.GetInt32(10);
                    selectedStyle = reader.GetInt32(11);
                    region = reader.GetInt32(12);
                    LocationX = reader.GetInt16(13);
                    LocationY = reader.GetInt16(14);
                    LocationZ = reader.GetInt16(15);
                    strength = reader.GetInt16(16);
                    dexterity = reader.GetInt16(17);
                    vitality = reader.GetInt16(18);
                    intelligence = reader.GetInt16(19);
                    spirit = reader.GetInt16(20);
                    skillStat1 = reader.GetInt16(21);
                    skillStat2 = reader.GetInt16(22);
                }
            }
        }


    }
}
