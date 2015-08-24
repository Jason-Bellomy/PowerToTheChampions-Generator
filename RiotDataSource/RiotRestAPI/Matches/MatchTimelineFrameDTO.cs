using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTimelineFrameDTO
    {
        [DataMember(Name = "participantFrames", Order = 0)]
        public MatchTimelineParticipantFramesDTO ParticipantFrames { get; set; }

        [DataMember(Name = "events", Order = 1)]
        public List<MatchTimelineFrameEventDTO> FrameEvents { get; set; }

        [DataMember(Name = "timestamp", Order = 2)]
        public long Timestamp { get; set; }
    }
}
