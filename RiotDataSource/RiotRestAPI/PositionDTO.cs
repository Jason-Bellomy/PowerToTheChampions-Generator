using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class PositionDTO
    {
        [DataMember(Name = "x", Order = 0)]
        public int X { get; set; }

        [DataMember(Name = "y", Order = 1)]
        public int Y { get; set; }
    }
}
