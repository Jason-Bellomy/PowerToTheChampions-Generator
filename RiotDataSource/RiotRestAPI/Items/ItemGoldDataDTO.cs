using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class ItemGoldDataDTO
    {
        [DataMember(Name = "base", Order = 0)]
        public int Base { get; set; }

        [DataMember(Name = "total", Order = 1)]
        public int Total { get; set; }

        [DataMember(Name = "sell", Order = 2)]
        public int Sell { get; set; }

        [DataMember(Name = "purchasable", Order = 3)]
        public bool IsPurchasable { get; set; }
    }
}
