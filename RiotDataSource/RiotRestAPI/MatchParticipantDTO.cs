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
        [DataMember(Name = "teamId")]
        public string TeamId { get; set; }

        [DataMember(Name = "spell1Id")]
        public string SpellOneId { get; set; }

        [DataMember(Name = "spell2Id")]
        public string SpellTwoId { get; set; }

        [DataMember(Name = "championId")]
        public string ChampionId { get; set; }

        [DataMember(Name = "highestAchievedSeasonTier")]
        public string AchievedTier { get; set; }

        // "timeline"

        // "masteries"

        // "stats"
        [DataMember(Name = "stats")]
        public MatchParticipantStatsDTO ParticipantStats { get; set; }

        [DataMember(Name = "participantId")]
        public string participantId { get; set; }

        // "runes"
    }
}
