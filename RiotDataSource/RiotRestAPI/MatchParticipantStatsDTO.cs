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
        [DataMember(Name = "winner")]
        public bool WinnerFlag { get; set; }


    }
}
