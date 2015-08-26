using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class ItemsDTO
    {
        [DataMember(Name = "type", Order = 0)]
        public string Type { get; set; }

        [DataMember(Name = "version", Order = 1)]
        public string Version { get; set; }

        // "basic" - node seems useless

        [DataMember(Name = "data", Order = 3)]
        public Dictionary<string, ItemDTO> Items { get; set; }
    }
}
