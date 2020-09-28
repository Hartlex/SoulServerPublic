using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SunCommon.Entities
{
    public class Character
    {


        public int Id { get; set; }
        public Account Account { get; set; }
        public int ClassCode { get; set; }
        public string CharName { get; set; }
        public int HeightCode { get; set; }
        public int FaceCode { get; set; }
        public int HairCode { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Vitality { get; set; }
        public int Intelligence { get; set; }
        public int Spirit { get; set; }
        public int SkillStat1 { get; set; }
        public int SkillStat2 { get; set; }
        public int UserPoint { get; set; }
        public long Experience { get; set; }
        public Single Hp { get; set; }
        public Single MaxHp { get; set; }
        public Single Mp { get; set; }
        public Single MaxMp { get; set; }
        public int RemainStat { get; set; }
        public int RemainSkill { get; set; }
        public int SelectedStyle { get; set; }
        public int PkState { get; set; }
        public int CharState { get; set; }
        public int StateTime { get; set; }
        public CharacterPosition CharacterPosition { get; set; }
        public int Slot { get; set; }
        public string TitleID { get; set; }
        public int InvisibleOpt { get; set; }
        public Inventory Inventory { get; set; }
        public byte[] Skill { get; set; }
        public byte[] Quick { get; set; }
        public byte[] Style { get; set; }
        public byte[] Quest { get; set; }
        public byte[] Mission { get; set; }
        public int PlayLimitedTime { get; set; }
        public int PvpPoint { get; set; }
        public int PvpScore { get; set; }
        public int PvpGrade { get; set; }
        public int PvpDraw { get; set; }
        public int PvpSeries { get; set; }
        public int PvpKill { get; set; }
        public int PvpDie { get; set; }
        public int PvpMaxKill { get; set; }
        public int PvpMaxDie { get; set; }
        public int GuildID { get; set; }
        public int GuildPosition { get; set; }
        public int GuildUserPoint { get; set; }
        public String GuildNickName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public long Money { get; set; }
        public long TitleTime { get; set; }
        public byte InvisOption { get; set; }
        public byte InventoryLock { get; set; }
        public long PlayLimitiedTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte DeleteCheck { get; set; }

    }

}
