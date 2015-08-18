using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantTimelineDTO
    {
        [DataMember(Name = "creepsPerMinDeltas", Order = 0)]
        MatchParticipantTimelineDeltaDTO CreepsPerMinDeltas { get; set; }

        [DataMember(Name = "xpPerMinDeltas", Order = 1)]
        MatchParticipantTimelineDeltaDTO XPPerMinDeltas { get; set; }

        [DataMember(Name = "goldPerMinDeltas", Order = 2)]
        MatchParticipantTimelineDeltaDTO GoldPerMinDeltas { get; set; }

        [DataMember(Name = "csDiffPerMinDeltas", Order = 3, IsRequired = false)]
        MatchParticipantTimelineDeltaDTO CSDiffPerMinDeltas { get; set; }

        [DataMember(Name = "xpDiffPerMinDeltas", Order = 4, IsRequired = false)]
        MatchParticipantTimelineDeltaDTO XPDiffPerMinDeltas { get; set; }

        [DataMember(Name = "damageTakenPerMinDeltas", Order = 5, IsRequired = false)]
        MatchParticipantTimelineDeltaDTO DamageTakenPerMinDeltas { get; set; }

        [DataMember(Name = "damageTakenDiffPerMinDeltas", Order = 6)]
        MatchParticipantTimelineDeltaDTO DamageTakenDiffPerMinDeltas { get; set; }

        [DataMember(Name = "role", Order = 7)]
        public string Role { get; set; }

        [DataMember(Name = "lane", Order = 8)]
        public string Lane { get; set; }
    }
}
