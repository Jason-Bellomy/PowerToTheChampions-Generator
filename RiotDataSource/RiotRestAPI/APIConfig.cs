﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.RiotRestAPI
{
    [DataContract]
    class APIConfig
    {
        [DataMember(Name = "apiKey", Order = 0)]
        public string ApiKey { get; set; }
    }
}
