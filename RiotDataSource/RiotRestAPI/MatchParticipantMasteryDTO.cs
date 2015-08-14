using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantMasteryDTO
    {
        [DataMember(Name = "masteryId", Order = 0)]
        public int MasteryId { get; set; }

        [DataMember(Name = "rank", Order = 1)]
        public int MasteryRank { get; set; }
    }
}
