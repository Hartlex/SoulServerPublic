using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;

namespace SunCommon.Entities
{
    public class Character
    {


        public int Id { get; set; }
        public Account Account { get; set; }
        public byte ClassCode { get; set; }
        public string CharName { get; set; }
        public byte HeightCode { get; set; }
        public byte FaceCode { get; set; }
        public byte HairCode { get; set; }
        public short Level { get; set; }
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
        public byte PkState { get; set; }
        public byte CharState { get; set; }
        public long StateTime { get; set; }
        public CharacterPosition CharacterPosition { get; set; }
        public int Slot { get; set; }
        public string TitleID { get; set; }
        public long TitleTime { get; set; }
        public byte InvisibleOpt { get; set; }
        public Inventory Inventory { get; set; }
        public byte[] Skill { get; set; }
        public byte[] Quick { get; set; }
        public byte[] Style { get; set; }
        public byte[] Quest { get; set; }
        public byte[] Mission { get; set; }
        public long PlayLimitedTime { get; set; }
        public int PvpPoint { get; set; }
        public int PvpScore { get; set; }
        public byte PvpGrade { get; set; }
        public int PvpDraw { get; set; }
        public int PvpSeries { get; set; }
        public int PvpKill { get; set; }
        public int PvpDie { get; set; }
        public int PvpMaxKill { get; set; }
        public int PvpMaxDie { get; set; }
        public int GuildID { get; set; }
        public byte GuildPosition { get; set; }
        public int GuildUserPoint { get; set; }
        public String GuildNickName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public byte DeleteCheck { get; set; }

        public Character(){}
        public Character(byte[] bytes)
        {
            ByteBuffer b = new ByteBuffer(bytes);
            Id = b.ReadInt32();
            Account = new Account(b.ReadInt32());
            Inventory = new Inventory();
            CharacterPosition = new CharacterPosition();
            Slot = b.ReadByte();
            ClassCode = b.ReadByte();
            var strhelper = b.ReadBlock(16);

            for (int i = 0; i < strhelper.Length; i++)
            {
                if (strhelper[i] == 0)
                {
                    byte[] help = new byte[i];
                    Array.Copy(strhelper, help, i);
                    CharName = Encoding.ASCII.GetString(help);
                    break;
                }
            }
            //CharName = Encoding.ASCII.GetString(b.ReadBlock(16));
            HeightCode = b.ReadByte();
            FaceCode = b.ReadByte();
            HairCode = b.ReadByte();
            Level = b.ReadInt16();
            Strength = b.ReadInt32();
            Dexterity = b.ReadInt32();
            Vitality = b.ReadInt32();
            Intelligence = b.ReadInt32();
            Spirit = b.ReadInt32();
            SkillStat1 = b.ReadInt32();
            SkillStat2 = b.ReadInt32();
            UserPoint = b.ReadInt32();
            Experience = b.ReadInt64();
            MaxHp = b.ReadSingle();
            Hp = b.ReadSingle();
            MaxMp = b.ReadSingle();
            Mp = b.ReadSingle();
            Inventory.Money = b.ReadInt64();
            RemainStat = b.ReadInt32();
            RemainSkill = b.ReadInt32();
            PkState = b.ReadByte();
            CharState = b.ReadByte();
            StateTime = b.ReadInt64();
            CharacterPosition.Region = b.ReadInt32();
            CharacterPosition.LocationX = b.ReadInt16();
            CharacterPosition.LocationY = b.ReadInt16();
            CharacterPosition.LocationZ = b.ReadInt16();
            strhelper = b.ReadBlock(16);
            for (int i = 0; i < strhelper.Length; i++)
            {
                if (strhelper[i] == 0)
                {
                    byte[] help = new byte[i];
                    Array.Copy(strhelper, help, i);
                    TitleID = Encoding.ASCII.GetString(help);
                    break;
                }
            }
            //TitleID = Encoding.ASCII.GetString(b.ReadBlock(16));
            TitleTime = b.ReadInt64();
            InvisibleOpt = b.ReadByte();
            Inventory.InventoryLock = b.ReadByte();
            var size = b.ReadInt32();
            Inventory.InventoryItem = b.ReadBlock(size);
            size = b.ReadInt32();
            Inventory.TmpInventoryItem = b.ReadBlock(size);
            size = b.ReadInt32();
            Inventory.EquipItem = b.ReadBlock(size);
            size = b.ReadInt32();
            Skill = b.ReadBlock(size);
            size = b.ReadInt32();
            Quick = b.ReadBlock(size);
            size = b.ReadInt32();
            Style = b.ReadBlock(size);
            size = b.ReadInt32();
            Quest = b.ReadBlock(size);
            size = b.ReadInt32();
            Mission = b.ReadBlock(size);
            PlayLimitedTime = b.ReadInt64();
            PvpPoint = b.ReadInt32();
            PvpScore = b.ReadInt32();
            PvpGrade = b.ReadByte();
            PvpDraw = b.ReadInt32();
            PvpSeries = b.ReadInt32();
            PvpKill = b.ReadInt32();
            PvpDie = b.ReadInt32();
            PvpMaxKill = b.ReadInt32();
            PvpMaxDie = b.ReadInt32();
            GuildID = b.ReadInt32();
            GuildPosition = b.ReadByte();
            GuildUserPoint = b.ReadInt32();
            strhelper = b.ReadBlock(16);
            //TODO convert this shit to a method somewhere
            for (int i = 0; i < strhelper.Length; i++)
            {
                if (strhelper[i] == 0)
                {
                    byte[] help = new byte[i];
                    Array.Copy(strhelper, help, i);
                    GuildNickName = Encoding.ASCII.GetString(help);
                    break;
                }
            }
            Inventory.SerializeInventoryByteStream();

        }


    }

}
