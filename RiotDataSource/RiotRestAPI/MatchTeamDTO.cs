using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTeamDTO
    {
        [DataMember(Name = "teamId")]
        public int TeamId { get; set; }
    }
}
