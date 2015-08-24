using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class ItemDTO
    {
        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "group", Order = 2)]
        public string Group { get; set; }

        [DataMember(Name = "description", Order = 3)]
        public string Description { get; set; }

        [DataMember(Name = "sanitizedDescription", Order = 4)]
        public string SanitizedDescription { get; set; }

        [DataMember(Name = "colloq", Order = 5)]
        public string Colloq { get; set; }

        [DataMember(Name = "plaintext", Order = 6)]
        public string ShortDescription { get; set; }

        [DataMember(Name = "depth", Order = 7)]
        public int Depth { get; set; }

        [DataMember(Name = "from", Order = 8)]
        public List<int> BuildsFromItemIds { get; set; }

        [DataMember(Name = "into", Order = 9)]
        public List<int> BuildsIntoItemIds { get; set; }

        [DataMember(Name = "tags", Order = 10)]
        public List<string> Tags { get; set; }

        [DataMember(Name = "image", Order = 11)]
        public ItemImageDataDTO ImageData { get; set; }

        [DataMember(Name = "stats", Order = 12)]
        public BasicDataStatsDTO Stats { get; set; }

        [DataMember(Name = "gold", Order = 13)]
        public ItemGoldDataDTO GoldData { get; set; }

        //[DataMember(Name = "effects", Order = 14)]
        //public ItemEffectsDTO Effects { get; set; }
    }
}
