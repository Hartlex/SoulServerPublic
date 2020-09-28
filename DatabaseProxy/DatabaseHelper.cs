using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SunCommon.Entities;

namespace DatabaseProxy
{
    public static class DatabaseHelper
    {
        public static SqlConnectionStringBuilder GetSqlConnectionString(
            string dataSource = @"KLS-AL-JPJVLQ2\sunonline",
            string userID = "sa",
            string passwort = "1yQA2xWS3cED4vRF",
            string initCatalog = "SoulServer",
            bool security = false)
        {
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = userID,
                Password = passwort,
                InitialCatalog = initCatalog,
                IntegratedSecurity = security
            };
            return sqlConnectionString;
        }

        public static string SelectString(string SelectColumn, string inTable, string conditionColumn,
            string conditionValue)
        {
            return "SELECT " + SelectColumn + " FROM dbo.[" + inTable + "] WHERE [" + conditionColumn + "]=" +
                            conditionValue;
        }

        public static SqlConnection DbConnection()
        {
            return new SqlConnection(GetSqlConnectionString().ConnectionString);
        }

        public static CharacterSetDBEntity GetCharacterSetDbEntity()
        {
            return null;
        }

        //public static Character getCharacterEntity(CharacterDBEntity d)
        //{
        //    var c = new Character();
        //    c.Id = d.charID;
        //    c.Account = new Account(d.userID);
        //    c.ClassCode = d.classCode;
        //    c.HeightCode = d.heightCode;
        //    c.FaceCode = d.faceCode;
        //    c.HairCode = d.hairCode;
        //    c.Level = d.level;
        //    c.Strength = d.strength;
        //    c.Dexterity = d.dexterity;
        //    c.Vitality = d.vitality;
        //    c.Intelligence = d.intelligence;
        //    c.Spirit = d.spirit;
        //    c.SkillStat1 = d.skillStat1;
        //    c.SkillStat2 = d.skillStat2;
        //    c.UserPoint = d.UserPoint;
        //    c.Experience = d.experience;
        //    c.Hp = d.Hp;
        //    c.MaxHp = d.MaxHp;
        //    c.Mp = d.Mp;
        //    c.MaxMp = d.MaxMp;
        //    c.RemainStat = d.RemainStat;
        //    c.RemainSkill = d.RemainSkill;
        //    c.SelectedStyle = d.selectedStyle;
        //    c.PkState = d.pkState;


        //}
        public static Character CreateCharacter(int slot,int userId, string charName, int heightCode, int faceCode, int hairCode, CharacterSetDBEntity set)
        {
            Character c = new Character();
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
            c.Money = set.money;
            c.Experience = set.experience;
            c.PkState = 1;
            c.CharState = 1;
            c.StateTime = 1;
            c.CharacterPosition = new CharacterPosition
            {
                Region = set.region,
                LocationX = set.LocationX,
                LocationZ = set.LocationZ,
                LocationY = set.LocationY,
                Angle = 0
            };
            c.TitleID = null;
            c.TitleTime = 1;
            c.InvisOption = 0;
            c.InventoryLock = 0;
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
            c.PlayLimitiedTime = 0;
            c.GuildNickName = null;
            c.CreationDate = DateTime.Now;
            c.ModifiedDate = DateTime.Now;
            c.LastLoginDate = DateTime.Now;
            c.DeleteCheck = 0;
            return c;
        }

        public static string CharacterTableColumns()
        {
            return    "(" +
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
                      "Intelligence,"+
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
    }


    public class CharacterSetDBEntity
    {
        public int charCode, classCode;
        public string className;
        public int level;
        public int UserPoint;
        public Single MaxHp, MaxMp;
        public long money;
        public int experience;
        public int RemainStat, RemainSkill, selectedStyle, region;
        public short LocationX, LocationY, LocationZ;
        public short strength, dexterity, vitality, intelligence, spirit, skillStat1, skillStat2;
        public byte[] inventoryItem = new byte[0];
        public byte[] tmpInventoryItem = new byte[0];
        public byte[] equipItem = new byte[0];
        public byte[] skill = new byte[0];
        public byte[] quick = new byte[0];
        public byte[] style = new byte[0];
        public byte[] quest = new byte[0];
        public byte[] mission = new byte[0];

        public CharacterSetDBEntity(int classCode)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseHelper.GetSqlConnectionString().ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DatabaseHelper.SelectString("*","CharacterSet","ClassCode",classCode.ToString()), conn);
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
    //public class CharacterDBEntity
    //{
    //    public int charID;
    //    public int userID;
    //    public int charSlot;
    //    public string charName;
    //    public int charCode, classCode, heightCode, faceCode, hairCode;
    //    public string className;
    //    public int level;
    //    public int UserPoint;
    //    public Single MaxHp, MaxMp, Hp, Mp;
    //    public long money;
    //    public int experience;
    //    public byte pkState;
    //    public byte charState;
    //    public long stateTime;
    //    public string titleID;
    //    public long titleTime;
    //    public byte invisOption;
    //    public byte inventoryLock;

    //    public int RemainStat, RemainSkill, selectedStyle, region, LocationX, LocationY, LocationZ;
    //    public short strength, dexterity, vitality, intelligence, spirit, skillStat1, skillStat2;
    //    byte[] inventoryItem = new byte[960];
    //    byte[] tmpInventoryItem = new byte[320];
    //    byte[] equipItem = new byte[336];
    //    byte[] skill = new byte[200];
    //    byte[] quick = new byte[192];
    //    byte[] style = new byte[8];
    //    byte[] quest = new byte[500];
    //    byte[] mission = new byte[128];
    //    public long playLimitiedTime;
    //    public int PVPPoint, PVPScore, PVPDraw, PVPSeries, PVPKill, PVPDeath, PVPMaxKill, PVPMaxDeath;
    //    public int PVPGrade;
    //    public int GuildID;
    //    public byte GuildPosition;
    //    public int GuildUserPoints;
    //    public string GuildNickName;
    //    public DateTime CreationDate, ModifiedDate, LastDate;
    //    public byte deleteCheck;
    //    public CharacterDBEntity(int userID, int charSlot,string charName, CharacterSetDBEntity set)
    //    {
    //        this.userID = userID;
    //        this.charSlot = charSlot;
    //        this.charCode = set.charCode;
    //        classCode = set.classCode;
    //        className = set.className;
    //        this.charName = charName;
    //        level = set.level;
    //        UserPoint = set.UserPoint;
    //        MaxHp = set.MaxHp;
    //        Hp = set.MaxHp;
    //        MaxMp = set.MaxMp;
    //        Mp = MaxMp;
    //        money = set.money;
    //        experience = set.experience;
    //        pkState = 1;
    //        charState = 1;
    //        stateTime = 1;
    //        titleID = null;
    //        titleTime = 1;
    //        invisOption = 0;
    //        inventoryLock = 0;
    //        RemainSkill = set.RemainSkill;
    //        RemainStat = set.RemainStat;
    //        selectedStyle = set.selectedStyle;
    //        region = set.region;
    //        LocationX = set.LocationX;
    //        LocationY = set.LocationY;
    //        LocationZ = set.LocationZ;
    //        strength = set.strength;
    //        dexterity = set.dexterity;
    //        vitality = set.vitality;
    //        intelligence = set.intelligence;
    //        spirit = set.spirit;
    //        skillStat1 = set.skillStat1;
    //        skillStat2 = set.skillStat2;
    //        playLimitiedTime = 0;
    //        GuildNickName = null;
    //        CreationDate = DateTime.Now;
    //        ModifiedDate = DateTime.Now;
    //        LastDate = DateTime.Now;
    //        deleteCheck = 0;

    //    }
        
    //}
}
