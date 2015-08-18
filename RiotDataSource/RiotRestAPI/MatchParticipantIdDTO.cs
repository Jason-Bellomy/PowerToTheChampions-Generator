using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantIdDTO
    {
        [DataMember(Name = "participantId", Order = 0)]
        public int ParticipantId { get; set; }
    }
}
