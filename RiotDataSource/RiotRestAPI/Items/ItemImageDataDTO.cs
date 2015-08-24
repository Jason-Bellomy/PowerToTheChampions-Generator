using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ItemImageDataDTO
    {
        [DataMember(Name = "full", Order = 0)]
        public string Full { get; set; }

        [DataMember(Name = "sprite", Order = 1)]
        public string Sprite { get; set; }

        [DataMember(Name = "group", Order = 2)]
        public string Group { get; set; }

        [DataMember(Name = "x", Order = 3)]
        public string X { get; set; }

        [DataMember(Name = "y", Order = 4)]
        public string Y { get; set; }

        [DataMember(Name = "w", Order = 5)]
        public string Width { get; set; }

        [DataMember(Name = "h", Order = 6)]
        public string Height { get; set; }
    }
}
