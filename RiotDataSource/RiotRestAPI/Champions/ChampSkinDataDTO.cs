using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampSkinDataDTO
    {
        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "num", Order = 1)]
        public string Num { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
    }
}
