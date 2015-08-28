using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampionsDTO
    {
        [DataMember(Name = "type", Order = 0)]
        public string Type { get; set; }

        [DataMember(Name = "version", Order = 1)]
        public string Version { get; set; }

        [DataMember(Name = "data", Order = 2)]
        public Dictionary<string, ChampionDTO> Champions { get; set; }
    }
}
