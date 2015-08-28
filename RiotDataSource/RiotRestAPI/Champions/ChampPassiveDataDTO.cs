using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampPassiveDataDTO
    {
        [DataMember(Name = "sanitizedDescription", Order = 0)]
        public string SanitizedDescription { get; set; }

        [DataMember(Name = "description", Order = 1)]
        public string Description { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "image", Order = 3)]
        public ChampImageDataDTO Image { get; set; }
    }
}
