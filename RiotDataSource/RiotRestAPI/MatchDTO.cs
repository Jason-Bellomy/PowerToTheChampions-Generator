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
        [DataMember(Name = "matchId")]
        public string MatchId { get; set; }

        [DataMember(Name = "region")]
        public string Region { get; set; }

        [DataMember(Name = "platformId")]
        public string PlatformId { get; set; }

        [DataMember(Name = "matchMode")]
        public string MatchMode { get; set; }

        [DataMember(Name = "matchType")]
        public string MatchType { get; set; }

        [DataMember(Name = "matchCreation")]
        public double MatchCreation { get; set; }

        [DataMember(Name = "matchDuration")]
        public double MatchDuration { get; set; }

        [DataMember(Name = "queueType")]
        public string QueueType { get; set; }

        [DataMember(Name = "mapId")]
        public int MapId { get; set; }

        [DataMember(Name = "season")]
        public string Season { get; set; }

        [DataMember(Name = "matchVersion")]
        public string MatchVersion { get; set; }

        [DataMember(Name = "participants")]
        public List<MatchParticipantDTO> MatchParticipants { get; set; }

        [DataMember(Name = "teams")]
        public List<MatchTeamDTO> MatchTeams { get; set; }
    }
}
