using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchDTO
    {
        [DataMember(Name = "matchId", Order = 0)]
        public string MatchId { get; set; }

        [DataMember(Name = "region", Order = 1)]
        public string Region { get; set; }

        [DataMember(Name = "platformId", Order = 2)]
        public string PlatformId { get; set; }

        [DataMember(Name = "matchMode", Order = 3)]
        public string MatchMode { get; set; }

        [DataMember(Name = "matchType", Order = 4)]
        public string MatchType { get; set; }

        [DataMember(Name = "matchCreation", Order = 5)]
        public double MatchCreation { get; set; }

        [DataMember(Name = "matchDuration", Order = 6)]
        public double MatchDuration { get; set; }

        [DataMember(Name = "queueType", Order = 7)]
        public string QueueType { get; set; }

        [DataMember(Name = "mapId", Order = 8)]
        public int MapId { get; set; }

        [DataMember(Name = "season", Order = 9)]
        public string Season { get; set; }

        [DataMember(Name = "matchVersion", Order = 10)]
        public string MatchVersion { get; set; }

        [DataMember(Name = "participants", Order = 11)]
        public List<MatchParticipantDTO> MatchParticipants { get; set; }

        [DataMember(Name = "participantIdentities", Order = 12)]
        public List<MatchParticipantIdDTO> MatchParticipantIds { get; set; }

        [DataMember(Name = "teams", Order = 13)]
        public List<MatchTeamDTO> MatchTeams { get; set; }
    }
}
