using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTeamDTO
    {
        [DataMember(Name = "teamId", Order = 0)]
        public int TeamId { get; set; }

        [DataMember(Name = "winner", Order = 1)]
        public bool Winner { get; set; }

        [DataMember(Name = "firstBlood", Order = 2)]
        public bool FirstBlood { get; set; }

        [DataMember(Name = "firstTower", Order = 3)]
        public bool FirstTower { get; set; }

        [DataMember(Name = "firstInhibitor", Order = 4)]
        public bool FirstInhibitor { get; set; }

        [DataMember(Name = "firstBaron", Order = 5)]
        public bool FirstBaron { get; set; }

        [DataMember(Name = "firstDragon", Order = 6)]
        public bool FirstDragon { get; set; }

        [DataMember(Name = "towerKills", Order = 7)]
        public int TowerKills { get; set; }

        [DataMember(Name = "inhibitorKills", Order = 8)]
        public int InhibitorKills { get; set; }

        [DataMember(Name = "baronKills", Order = 9)]
        public int BaronKills { get; set; }

        [DataMember(Name = "dragonKills", Order = 10)]
        public int DragonKills { get; set; }

        [DataMember(Name = "vilemawKills", Order = 11)]
        public int VilemawKills { get; set; }

        [DataMember(Name = "dominionVictoryScore", Order = 12)]
        public int DominionVictoryScore { get; set; }
    }
}
