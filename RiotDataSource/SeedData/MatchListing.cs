using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.SeedData
{
    [DataContract]
    class MatchListing
    {
        public MatchListing()
        {
            MatchIds = new List<string>();
        }

        [DataMember]
        public string version { get; set; }

        [DataMember]
        public string mode { get; set; }

        [DataMember]
        public string region { get; set; }

        [DataMember]
        public List<string> MatchIds { get; set; }
    }
}
