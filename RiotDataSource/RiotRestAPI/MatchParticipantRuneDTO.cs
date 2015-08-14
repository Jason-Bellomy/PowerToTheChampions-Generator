using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantRuneDTO
    {
        [DataMember(Name = "runeId", Order = 0)]
        public int RuneId { get; set; }

        [DataMember(Name = "rank", Order = 1)]
        public int Rank { get; set; }
    }
}
