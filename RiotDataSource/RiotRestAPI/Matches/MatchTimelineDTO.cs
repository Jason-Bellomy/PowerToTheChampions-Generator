using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTimelineDTO
    {
        [DataMember(Name = "frames", Order = 0)]
        public List<MatchTimelineFrameDTO> Frames { get; set; }

        [DataMember(Name = "frameInterval", Order = 1)]
        public long FrameInterval { get; set; }
    }
}
