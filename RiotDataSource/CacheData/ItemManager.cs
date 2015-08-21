using RiotDataSource.RiotRestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class ItemManager
    {
        const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private string _rawItemDataDirectory = "";
        private string _itemDataDTODirectory = "";

        private APIConnection _apiConnection = null;

        public ItemManager(APIConnection apiConnection, string rawItemDataDirectory, string itemDataDTODirectory)
        {
            _apiConnection = apiConnection;
            _rawItemDataDirectory = rawItemDataDirectory;
            _itemDataDTODirectory = itemDataDTODirectory;
        }
    }
}
