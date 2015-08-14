using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantDTO
    {
        [DataMember(Name = "teamId", Order = 0)]
        public string TeamId { get; set; }

        [DataMember(Name = "spell1Id", Order = 1)]
        public string SpellOneId { get; set; }

        [DataMember(Name = "spell2Id", Order = 2)]
        public string SpellTwoId { get; set; }

        [DataMember(Name = "championId", Order = 3)]
        public string ChampionId { get; set; }

        [DataMember(Name = "highestAchievedSeasonTier", Order = 4)]
        public string AchievedTier { get; set; }

        [DataMember(Name = "timeline", Order = 5)]
        public MatchParticipantTimelineDTO Timeline { get; set; }

        [DataMember(Name = "masteries", Order = 6)]
        public List<MatchParticipantMasteryDTO> Masteries { get; set; }

        [DataMember(Name = "stats", Order = 7)]
        public MatchParticipantStatsDTO ParticipantStats { get; set; }

        [DataMember(Name = "participantId", Order = 8)]
        public string ParticipantId { get; set; }

        [DataMember(Name = "runes", Order = 9)]
        public List<MatchParticipantRuneDTO> Runes { get; set; }
    }
}
