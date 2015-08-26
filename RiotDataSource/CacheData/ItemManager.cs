using Newtonsoft.Json;
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

        public List<int> LoadItemIds(string region, string version)
        {
            List<int> itemIds = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/static-data/" + region + "/v1.2/item/";

                string rawResponse = "";

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams["version"] = version;
                _apiConnection.Get<RiotRestAPI.ItemsDTO>(resource, queryParams, ref rateLimitHit, ref rawResponse);

                // Use JSON.NET serializer for this, because DataSerializer doesn't get the dictionary right
                ItemsDTO items = JsonConvert.DeserializeObject<ItemsDTO>(rawResponse);
                if (items != null)
                {
                    LogManager.LogMessage("Loaded items for " + region + "-" + version + " from the API.");

                    itemIds = new List<int>();
                    foreach (string itemId in items.Items.Keys)
                    {
                        itemIds.Add(Convert.ToInt32(itemId));
                        itemIds.Sort();
                    }
                }
                else if (rateLimitHit)
                {
                    LogManager.LogMessage("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogManager.LogMessage("Unable to load item list for " + region + "-" + version);
                }
            }

            return itemIds;
        }

        public RiotRestAPI.ItemDTO LoadItem(string region, string version, int itemId)
        {
            RiotRestAPI.ItemDTO item = null;

            if (DoesRawCachedCopyOfItemExist(region, version, itemId))
            {
                item = LoadItemFromRawFile(region, version, itemId);
            }
            else
            {
                string rawResponse = "";
                item = LoadItemFromAPI(region, version, itemId, ref rawResponse);

                if (item != null)
                {
                    // Save the raw response file to speed up future queries
                    string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(rawResponse);

                    string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, version, itemId) };
                    string filePath = System.IO.Path.Combine(rawfilePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
                    fstream.Write(data, 0, data.Length);
                    fstream.Close();
                }
            }

            return item;
        }

        private string GenerateRawItemFileName(string region, string version, int itemId)
        {
            return "item-" + region + "-v" + version.Replace('.', '-') + "-" + Convert.ToString(itemId) + ".json";
        }

        private ItemDTO LoadItemFromAPI(string region, string version, int itemId, ref string rawResponse)
        {
            RiotRestAPI.ItemDTO item = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/static-data/" + region + "/v1.2/item/" + itemId;

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams["version"] = version;
                queryParams["itemData"] = "all";
                item = _apiConnection.Get<RiotRestAPI.ItemDTO>(resource, queryParams,
                                                               ref rateLimitHit, ref rawResponse);
                if (item != null)
                {
                    LogManager.LogMessage("Loaded item " + region + "-" + version + "-" + itemId + " from the API.");
                }
                else if (rateLimitHit)
                {
                    LogManager.LogMessage("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogManager.LogMessage("Unable to load item: " + region + "-" + version + "-" + itemId);
                }
            }

            return item;
        }

        private ItemDTO LoadItemFromRawFile(string region, string version, int itemId)
        {
            RiotRestAPI.ItemDTO item = null;

            string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, version, itemId) };
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
                LogManager.LogMessage("XML Exception parsing item response: " + region + "-" + version + "-" + itemId + " - " + ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.LogMessage("Generic Exception parsing item response: " + region + "-" + version + "-" + itemId + " - " + ex.Message);
            }

            fstream.Close();

            if (objResponse == null)
            {
                LogManager.LogMessage("Failed to load item " + region + "-" + version + "-" + itemId + " from cached data. Deleting file.");
                File.Delete(filePath);
            }
            else
            {
                item = (RiotRestAPI.ItemDTO)Convert.ChangeType(objResponse, typeof(RiotRestAPI.ItemDTO));
                LogManager.LogMessage("Loaded item " + region + "-" + version + "-" + itemId + " from cached data.");
            }

            return item;
        }

        private bool DoesRawCachedCopyOfItemExist(string region, string version, int itemId)
        {
            string[] rawfilePathParts = new string[] { _rawItemDataDirectory, GenerateRawItemFileName(region, version, itemId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            return File.Exists(filePath);
        }
    }
}
