using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchParticipantTimelineCreepsDTO
    {
        [DataMember(Name = "zeroToTen", Order = 0)]
        public double ZeroToTen { get; set; }

        [DataMember(Name = "tenToTwenty", Order = 1)]
        public double TenToTwenty { get; set; }

        [DataMember(Name = "twentyToThirty", Order = 2)]
        public double TwentyToThirty { get; set; }

        [DataMember(Name = "thirtyToEnd", Order = 3)]
        public double ThirtyToEnd { get; set; }
    }
}
