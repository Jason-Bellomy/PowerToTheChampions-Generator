using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampInfoDataDTO
    {
        [DataMember(Name = "defense", Order = 0)]
        public int Defense { get; set; }

        [DataMember(Name = "magic", Order = 1)]
        public int Magic { get; set; }

        [DataMember(Name = "difficulty", Order = 2)]
        public int Difficulty { get; set; }

        [DataMember(Name = "attack", Order = 3)]
        public int Attack { get; set; }
    }
}
