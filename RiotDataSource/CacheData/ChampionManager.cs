using RiotDataSource.Logging;
using RiotDataSource.RiotRestAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class ChampionManager
    {
        const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private string _rawChampionDataDirectory = "";

        private APIConnection _apiConnection = null;

        public ChampionManager(APIConnection apiConnection, string rawChampionDataDirectory)
        {
            _apiConnection = apiConnection;
            _rawChampionDataDirectory = rawChampionDataDirectory;
        }

        public RiotRestAPI.ChampionDTO LoadChampion(string region, int championId)
        {
            RiotRestAPI.ChampionDTO champion = null;

            if (DoesRawCachedCopyOfChampionExist(region, championId))
            {
                champion = LoadChampionFromRawFile(region, championId);
            }
            else
            {
                string rawResponse = "";
                champion = LoadChampionFromAPI(region, championId, ref rawResponse);

                if (champion != null)
                {
                    // Save the raw response file to speed up future queries
                    string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(rawResponse);

                    string[] rawfilePathParts = new string[] { _rawChampionDataDirectory, GenerateRawChampionFileName(region, championId) };
                    string filePath = System.IO.Path.Combine(rawfilePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
                    fstream.Write(data, 0, data.Length);
                    fstream.Close();
                }
            }

            return champion;
        }

        private string GenerateRawChampionFileName(string region, int championId)
        {
            return "champion-" + region + "-" + Convert.ToString(championId) + ".json";
        }

        private ChampionDTO LoadChampionFromAPI(string region, int championId, ref string rawResponse)
        {
            RiotRestAPI.ChampionDTO champion = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/static-data/" + region + "/v1.2/champion/" + championId;

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams["champData"] = "all";
                champion = _apiConnection.Get<RiotRestAPI.ChampionDTO>(resource, queryParams,
                                                                 ref rateLimitHit, ref rawResponse);
                if (champion != null)
                {
                    LogManager.LogMessage("Loaded champion " + region + "-" + championId + " from the API.");
                }
                else if (rateLimitHit)
                {
                    LogManager.LogMessage("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogManager.LogMessage("Unable to load champ: " + region + " - " + championId);
                }
            }

            return champion;
        }

        private ChampionDTO LoadChampionFromRawFile(string region, int championId)
        {
            RiotRestAPI.ChampionDTO champion = null;

            string[] rawfilePathParts = new string[] { _rawChampionDataDirectory, GenerateRawChampionFileName(region, championId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            FileStream fstream = new FileStream(filePath, FileMode.Open);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RiotRestAPI.ChampionDTO));
            object objResponse = null;
            try
            {
                objResponse = jsonSerializer.ReadObject(fstream);
            }
            catch (System.Xml.XmlException ex)
            {
                LogManager.LogMessage("XML Exception while parsing champion response: " + region + "-" + championId + " - " + ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.LogMessage("Generic Exception while parsing champion response: " + region + "-" + championId + " - " + ex.Message);
            }

            fstream.Close();

            if (objResponse == null)
            {
                LogManager.LogMessage("Failed to load champion " + region + "-" + championId + " from cached data. Deleting file.");
                File.Delete(filePath);
            }
            else
            {
                champion = (RiotRestAPI.ChampionDTO)Convert.ChangeType(objResponse, typeof(RiotRestAPI.ChampionDTO));
                LogManager.LogMessage("Loaded champion " + region + "-" + championId + " from cached data.");
            }

            return champion;
        }

        private bool DoesRawCachedCopyOfChampionExist(string region, int championId)
        {
            string[] rawfilePathParts = new string[] { _rawChampionDataDirectory, GenerateRawChampionFileName(region, championId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            return File.Exists(filePath);
        }
    }
}
