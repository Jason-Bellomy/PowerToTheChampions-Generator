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
    class ItemManager
    {
        const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private string _rawItemDataDirectory = "";

        private APIConnection _apiConnection = null;

        public ItemManager(APIConnection apiConnection, string rawItemDataDirectory)
        {
            _apiConnection = apiConnection;
            _rawItemDataDirectory = rawItemDataDirectory;
        }

        public RiotRestAPI.ItemDTO LoadItem(string region, int itemId)
        {
            RiotRestAPI.ItemDTO item = null;

            if (DoesRawCachedCopyOfItemExist(region, itemId))
            {
                item = LoadItemFromRawFile(region, itemId);
            }
            else
            {
                string rawResponse = "";
                item = LoadItemFromAPI(region, itemId, ref rawResponse);

                if (item != null)
                {
                    // Save the raw response file to speed up future queries
                    string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(rawResponse);

                    string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, itemId) };
                    string filePath = System.IO.Path.Combine(rawfilePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
                    fstream.Write(data, 0, data.Length);
                    fstream.Close();
                }
            }

            return item;
        }

        private string GenerateRawItemFileName(string region, int itemId)
        {
            return "item-" + region + "-" + Convert.ToString(itemId) + ".json";
        }

        private ItemDTO LoadItemFromAPI(string region, int itemId, ref string rawResponse)
        {
            RiotRestAPI.ItemDTO item = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/static-data/" + region + "/v1.2/item/" + itemId;

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams["itemData"] = "all";
                item = _apiConnection.Get<RiotRestAPI.ItemDTO>(resource, queryParams,
                                                                 ref rateLimitHit, ref rawResponse);
                if (item != null)
                {
                    LogManager.LogMessage("Loaded item " + region + "-" + itemId + " from the API.");
                }
                else if (rateLimitHit)
                {
                    LogManager.LogMessage("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogManager.LogMessage("Unable to load item: " + region + " - " + itemId);
                }
            }

            return item;
        }

        private ItemDTO LoadItemFromRawFile(string region, int itemId)
        {
            RiotRestAPI.ItemDTO item = null;

            string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, itemId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            FileStream fstream = new FileStream(filePath, FileMode.Open);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RiotRestAPI.ItemDTO));
            object objResponse = null;
            try
            {
                objResponse = jsonSerializer.ReadObject(fstream);
            }
            catch (System.Xml.XmlException ex)
            {
                LogManager.LogMessage("XML Exception while parsing item response: " + region + "-" + itemId + " - " + ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.LogMessage("Generic Exception while parsing item response: " + region + "-" + itemId + " - " + ex.Message);
            }

            fstream.Close();

            if (objResponse == null)
            {
                LogManager.LogMessage("Failed to load item " + region + "-" + itemId + " from cached data. Deleting file.");
                File.Delete(filePath);
            }
            else
            {
                item = (RiotRestAPI.ItemDTO)Convert.ChangeType(objResponse, typeof(RiotRestAPI.ItemDTO));
                LogManager.LogMessage("Loaded item " + region + "-" + itemId + " from cached data.");
            }

            return item;
        }

        private bool DoesRawCachedCopyOfItemExist(string region, int itemId)
        {
            string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, itemId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            return File.Exists(filePath);
        }
    }
}
