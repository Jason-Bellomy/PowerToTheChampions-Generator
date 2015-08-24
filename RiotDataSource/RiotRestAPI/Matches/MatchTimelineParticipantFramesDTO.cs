using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTimelineParticipantFramesDTO
    {
        [DataMember(Name = "1")]
        public MatchTimelineParticipantFrameDTO Participant1 { get; set; }

        [DataMember(Name = "2")]
        public MatchTimelineParticipantFrameDTO Participant2 { get; set; }

        [DataMember(Name = "3")]
        public MatchTimelineParticipantFrameDTO Participant3 { get; set; }

        [DataMember(Name = "4")]
        public MatchTimelineParticipantFrameDTO Participant4 { get; set; }

        [DataMember(Name = "5")]
        public MatchTimelineParticipantFrameDTO Participant5 { get; set; }

        [DataMember(Name = "6")]
        public MatchTimelineParticipantFrameDTO Participant6 { get; set; }

        [DataMember(Name = "7")]
        public MatchTimelineParticipantFrameDTO Participant7 { get; set; }

        [DataMember(Name = "8")]
        public MatchTimelineParticipantFrameDTO Participant8 { get; set; }

        [DataMember(Name = "9")]
        public MatchTimelineParticipantFrameDTO Participant9 { get; set; }

        [DataMember(Name = "10")]
        public MatchTimelineParticipantFrameDTO Participant10 { get; set; }
    }
}
