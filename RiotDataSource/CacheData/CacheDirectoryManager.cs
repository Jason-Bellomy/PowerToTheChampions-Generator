using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class CacheDirectoryManager
    {
        private string _rootDirectoryPath = "";

        public CacheDirectoryManager(string rootCacheDirectoryPath)
        {
            _rootDirectoryPath = rootCacheDirectoryPath;
        }

        public void VerifyFolderStructure()
        {
            if (!Directory.Exists(_rootDirectoryPath))
            {
                Directory.CreateDirectory(_rootDirectoryPath);
            }

            if (!Directory.Exists(APIConfigFolderPath))
            {
                Directory.CreateDirectory(APIConfigFolderPath);
            }

            if (!Directory.Exists(MatchDataFolderPath))
            {
                Directory.CreateDirectory(MatchDataFolderPath);
            }

            if (!Directory.Exists(ItemDataFolderPath))
            {
                Directory.CreateDirectory(ItemDataFolderPath);
            }

            if (!Directory.Exists(ChampionDataFolderPath))
            {
                Directory.CreateDirectory(ChampionDataFolderPath);
            }
        }

        public string APIConfigFolderPath
        {
            get
            {
                string[] pathsToAPIConfig = new string[] { _rootDirectoryPath, "ApiConfig" };
                return System.IO.Path.Combine(pathsToAPIConfig);
            }
        }

        public string APIConfigFilePath
        {
            get
            {
                string[] pathsToAPIConfig = new string[] { APIConfigFolderPath, "apiConfig.json" };
                return System.IO.Path.Combine(pathsToAPIConfig);
            }
        }

        public string MatchDataFolderPath
        {
            get
            {
                return System.IO.Path.Combine(_rootDirectoryPath, "RawMatchData");
            }
        }

        public string ItemDataFolderPath
        {
            get
            {
                return System.IO.Path.Combine(_rootDirectoryPath, "RawItemData");
            }
        }

        public string ChampionDataFolderPath
        {
            get
            {
                return System.IO.Path.Combine(_rootDirectoryPath, "RawChampionData");
            }
        }
    }
}
