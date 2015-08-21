using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.SeedData
{
    [DataContract]
    public class MatchListing
    {
        public MatchListing()
        {
            MatchIds = new List<string>();
        }

        public string DisplayName
        {
            get
            {
                return region + "-" + mode + "-" + version;
            }
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
