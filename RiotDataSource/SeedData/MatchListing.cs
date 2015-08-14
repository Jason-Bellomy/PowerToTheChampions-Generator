using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.SeedData
{
    class MatchListing
    {
        public MatchListing()
        {
            MatchIds = new List<string>();
        }

        public string version { get; set; }
        public string mode { get; set; }
        public string region { get; set; }
        public List<string> MatchIds { get; set; }
    }
}
