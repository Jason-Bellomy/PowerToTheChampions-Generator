using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class MatchTimelineFrameEventDTO
    {
        [DataMember(Name = "ascendedType", Order = 0)]
        public string AscendedType { get; set; }

        [DataMember(Name = "assistingParticipantIds", Order = 1)]
        public List<int> AssistingParticipantIds { get; set; }

        [DataMember(Name = "buildingType", Order = 2)]
        public string BuildingType { get; set; }

        [DataMember(Name = "creatorId", Order = 3)]
        public int CreatorId { get; set; }

        [DataMember(Name = "eventType", Order = 4, IsRequired = true)]
        public string EventType { get; set; }

        [DataMember(Name = "itemAfter", Order = 5)]
        public int ItemAfter { get; set; }

        [DataMember(Name = "itemBefore", Order = 6)]
        public int ItemBefore { get; set; }

        [DataMember(Name = "itemId", Order = 7)]
        public int ItemId { get; set; }

        [DataMember(Name = "killerId", Order = 8)]
        public int KillerId { get; set; }

        [DataMember(Name = "laneType", Order = 9)]
        public string LaneType { get; set; }

        [DataMember(Name = "levelUpType", Order = 10)]
        public string LevelUpType { get; set; }

        [DataMember(Name = "monsterType", Order = 11)]
        public string MonsterType { get; set; }

        [DataMember(Name = "participantId", Order = 12)]
        public int ParticipantId { get; set; }

        [DataMember(Name = "pointCaptured", Order = 13)]
        public string PointCaptured { get; set; }
       
        [DataMember(Name = "position", Order = 14)]
        public PositionDTO Position { get; set; }

        [DataMember(Name = "skillSlot", Order = 15)]
        public int SkillSlot { get; set; }

        [DataMember(Name = "teamId", Order = 16)]
        public int TeamId { get; set; }

        [DataMember(Name = "timestamp", Order = 17)]
        public long Timestamp { get; set; }

        [DataMember(Name = "towerType", Order = 18)]
        public string TowerType { get; set; }

        [DataMember(Name = "victimId", Order = 19)]
        public int VictimId { get; set; }

        [DataMember(Name = "wardType", Order = 20)]
        public string WardType { get; set; }
    }
}
