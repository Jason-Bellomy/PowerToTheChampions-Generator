using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    public class ChampionDTO
    {
        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "title", Order = 1)]
        public string Title { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "stats", Order = 3)]
        public ChampStatsDataDTO Stats { get; set; }

        [DataMember(Name = "key", Order = 4)]
        public string Key { get; set; }

        [DataMember(Name = "skins", Order = 5)]
        public List<ChampSkinDataDTO> Skins { get; set; }

        [DataMember(Name = "partype", Order = 6)]
        public string Partype { get; set; }

        [DataMember(Name = "passive", Order = 7)]
        public ChampPassiveDataDTO Passive { get; set; }

        [DataMember(Name = "lore", Order = 8)]
        public string Lore { get; set; }

        [DataMember(Name = "info", Order = 9)]
        public ChampInfoDataDTO Info { get; set; }

        [DataMember(Name = "image", Order = 10)]
        public ChampImageDataDTO Image { get; set; }

        [DataMember(Name = "tags", Order = 11)]
        public List<string> Tags { get; set; }
    }
}
