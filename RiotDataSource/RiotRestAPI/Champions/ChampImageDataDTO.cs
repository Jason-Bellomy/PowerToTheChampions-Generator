using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampImageDataDTO
    {
        [DataMember(Name = "w", Order = 0)]
        public string W { get; set; }

        [DataMember(Name = "full", Order = 1)]
        public string Full { get; set; }

        [DataMember(Name = "sprite", Order = 2)]
        public string Sprite { get; set; }

        [DataMember(Name = "group", Order = 3)]
        public string Group { get; set; }

        [DataMember(Name = "h", Order = 4)]
        public string H { get; set; }

        [DataMember(Name = "y", Order = 5)]
        public string Y { get; set; }

        [DataMember(Name = "x", Order = 6)]
        public string X { get; set; }
    }
}
