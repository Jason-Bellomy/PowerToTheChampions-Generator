using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampStatsDataDTO
    {
        [DataMember(Name = "attackrange", Order = 0)]
        public int AttackRange { get; set; }

        [DataMember(Name = "mpperlevel", Order = 1)]
        public float MpPerLevel { get; set; }

        [DataMember(Name = "mp", Order = 2)]
        public float Mp { get; set; }

        [DataMember(Name = "attackdamage", Order = 3)]
        public float AttackDamage { get; set; }

        [DataMember(Name = "hp", Order = 4)]
        public float Hp { get; set; }

        [DataMember(Name = "hpperlevel", Order = 5)]
        public float HpPerLevel { get; set; }

        [DataMember(Name = "attackdamageperlevel", Order = 6)]
        public float AttackDamagePerLevel { get; set; }

        [DataMember(Name = "armor", Order = 7)]
        public float Armor { get; set; }

        [DataMember(Name = "mpregenperlevel", Order = 8)]
        public float MpRegenPerLevel { get; set; }

        [DataMember(Name = "hpregen", Order = 9)]
        public float HpRegen { get; set; }

        [DataMember(Name = "critperlevel", Order = 10)]
        public float CritPerLevel { get; set; }

        [DataMember(Name = "spellblockperlevel", Order = 11)]
        public float SpellBlockPerLevel { get; set; }

        [DataMember(Name = "mpregen", Order = 12)]
        public float MpRegen { get; set; }

        [DataMember(Name = "attackspeedperlevel", Order = 13)]
        public float AttackspeedPerLevel { get; set; }

        [DataMember(Name = "spellblock", Order = 14)]
        public float SpellBlock { get; set; }

        [DataMember(Name = "movespeed", Order = 15)]
        public int MoveSpeed { get; set; }

        [DataMember(Name = "attackspeedoffset", Order = 16)]
        public float AttackspeedOffset { get; set; }

        [DataMember(Name = "crit", Order = 17)]
        public float Crit { get; set; }

        [DataMember(Name = "hpregenperlevel", Order = 18)]
        public float HpRegenPerLevel { get; set; }

        [DataMember(Name = "armorperlevel", Order = 19)]
        public float ArmorPerLevel { get; set; }
    }
}
