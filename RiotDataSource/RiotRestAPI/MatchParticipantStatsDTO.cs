using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantStatsDTO
    {
        [DataMember(Name = "winner", Order = 0)]
        public bool WinnerFlag { get; set; }

        [DataMember(Name = "champLevel", Order = 1)]
        public int ChampionLevel { get; set; }

        [DataMember(Name = "item0", Order = 2)]
        public int Item0 { get; set; }

        [DataMember(Name = "item1", Order = 3)]
        public int Item1 { get; set; }

        [DataMember(Name = "item2", Order = 4)]
        public int Item2 { get; set; }

        [DataMember(Name = "item3", Order = 5)]
        public int Item3 { get; set; }

        [DataMember(Name = "item4", Order = 6)]
        public int Item4 { get; set; }

        [DataMember(Name = "item5", Order = 7)]
        public int Item5 { get; set; }

        [DataMember(Name = "item6", Order = 8)]
        public int Item6 { get; set; }

        [DataMember(Name = "kills", Order = 9)]
        public int Kills { get; set; }

        [DataMember(Name = "doubleKills", Order = 10)]
        public int DoubleKills { get; set; }

        [DataMember(Name = "tripleKills", Order = 11)]
        public int TripleKills { get; set; }

        [DataMember(Name = "quadraKills", Order = 12)]
        public int QuadraKills { get; set; }

        [DataMember(Name = "pentaKills", Order = 13)]
        public int PentaKills { get; set; }

        [DataMember(Name = "unrealKills", Order = 14)]
        public int UnrealKills { get; set; }

        [DataMember(Name = "largestKillingSpree", Order = 15)]
        public int LargestKillingSpree { get; set; }

        [DataMember(Name = "deaths", Order = 16)]
        public int Deaths { get; set; }

        [DataMember(Name = "assists", Order = 17)]
        public int Assists { get; set; }

        [DataMember(Name = "totalDamageDealt", Order = 18)]
        public int TotalDamageDealt { get; set; }

        [DataMember(Name = "totalDamageDealtToChampions", Order = 19)]
        public int TotalDamageDealtToChampions { get; set; }

        [DataMember(Name = "totalDamageTaken", Order = 20)]
        public int TotalDamageTaken { get; set; }

        [DataMember(Name = "largestCriticalStrike", Order = 21)]
        public int LargestCriticalStrike { get; set; }

        [DataMember(Name = "totalHeal", Order = 22)]
        public int TotalHealing { get; set; }

        [DataMember(Name = "minionsKilled", Order = 23)]
        public int MinionsKilled { get; set; }

        [DataMember(Name = "neutralMinionsKilled", Order = 24)]
        public int NeutralMinionsKilled { get; set; }

        [DataMember(Name = "neutralMinionsKilledTeamJungle", Order = 25)]
        public int NeutralMinionsKilledTeamJungle { get; set; }

        [DataMember(Name = "neutralMinionsKilledEnemyJungle", Order = 26)]
        public int NeutralMinionsKilledEnemyJungle { get; set; }

        [DataMember(Name = "goldEarned", Order = 27)]
        public int GoldEarned { get; set; }

        [DataMember(Name = "goldSpent", Order = 28)]
        public int GoldSpent { get; set; }

        [DataMember(Name = "combatPlayerScore", Order = 29)]
        public int CombatPlayerScore { get; set; }

        [DataMember(Name = "objectivePlayerScore", Order = 30)]
        public int ObjectivePlayerScore { get; set; }

        [DataMember(Name = "totalPlayerScore", Order = 31)]
        public int TotalPlayerScore { get; set; }

        [DataMember(Name = "totalScoreRank", Order = 32)]
        public int TotalScoreRank { get; set; }

        [DataMember(Name = "magicDamageDealtToChampions", Order = 33)]
        public int MagicDamageDealtToChampions { get; set; }

        [DataMember(Name = "physicalDamageDealtToChampions", Order = 34)]
        public int PhysicalDamageDealtToChampions { get; set; }

        [DataMember(Name = "trueDamageDealtToChampions", Order = 35)]
        public int TrueDamageDealtToChampions { get; set; }

        [DataMember(Name = "visionWardsBoughtInGame", Order = 36)]
        public int VisionWardsBoughtInGame { get; set; }

        [DataMember(Name = "sightWardsBoughtInGame", Order = 37)]
        public int SightWardsBoughtInGame { get; set; }

        [DataMember(Name = "magicDamageDealt", Order = 38)]
        public int MagicDamageDealt { get; set; }

        [DataMember(Name = "physicalDamageDealt", Order = 39)]
        public int PhysicalDamageDealt { get; set; }

        [DataMember(Name = "trueDamageDealt", Order = 40)]
        public int TrueDamageDealt { get; set; }

        [DataMember(Name = "magicDamageTaken", Order = 41)]
        public int MagicDamageTaken { get; set; }

        [DataMember(Name = "physicalDamageTaken", Order = 42)]
        public int PhysicalDamageTaken { get; set; }

        [DataMember(Name = "trueDamageTaken", Order = 43)]
        public int TrueDamageTaken { get; set; }

        [DataMember(Name = "firstBloodKill", Order = 44)]
        public bool FirstBloodKill { get; set; }

        [DataMember(Name = "firstBloodAssist", Order = 45)]
        public bool FirstBloodAssist { get; set; }

        [DataMember(Name = "firstTowerKill", Order = 46)]
        public bool FirstTowerKill { get; set; }

        [DataMember(Name = "firstTowerAssist", Order = 47)]
        public bool FirstTowerAssist { get; set; }

        [DataMember(Name = "firstInhibitorKill", Order = 48)]
        public bool FirstInhibitorKill { get; set; }

        [DataMember(Name = "firstInhibitorAssist", Order = 49)]
        public bool FirstInhibitorAssist { get; set; }

        [DataMember(Name = "inhibitorKills", Order = 50)]
        public int InhibitorKills { get; set; }

        [DataMember(Name = "towerKills", Order = 51)]
        public int TowerKills { get; set; }

        [DataMember(Name = "wardsPlaced", Order = 52)]
        public int WardsPlaced { get; set; }

        [DataMember(Name = "wardsKilled", Order = 53)]
        public int WardsKilled { get; set; }

        [DataMember(Name = "largestMultiKill", Order = 54)]
        public int LargestMultiKill { get; set; }

        [DataMember(Name = "killingSprees", Order = 55)]
        public int KillingSprees { get; set; }

        [DataMember(Name = "totalUnitsHealed", Order = 56)]
        public int TotalUnitsHealed { get; set; }

        [DataMember(Name = "totalTimeCrowdControlDealt", Order = 57)]
        public int TotalTimeCrowdControlDealt { get; set; }
    }
}
