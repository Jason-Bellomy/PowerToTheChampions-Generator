using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTimelineParticipantFrameDTO
    {
        [DataMember(Name = "currentGold", Order = 0)]
        public int CurrentGold { get; set; }

        [DataMember(Name = "dominionScore", Order = 1)]
        public int DominionScore { get; set; }

        [DataMember(Name = "jungleMinionsKilled", Order = 2)]
        public int JungleMinionsKilled { get; set; }

        [DataMember(Name = "level", Order = 3)]
        public int Level { get; set; }

        [DataMember(Name = "minionsKilled", Order = 4)]
        public int MinionsKilled { get; set; }

        [DataMember(Name = "position", Order = 5)]
        public PositionDTO Position { get; set; }

        [DataMember(Name = "teamScore", Order = 6)]
        public int TeamScore { get; set; }

        [DataMember(Name = "totalGold", Order = 7)]
        public int TotalGold { get; set; }

        [DataMember(Name = "xp", Order = 8)]
        public int XP { get; set; }
    }
}
