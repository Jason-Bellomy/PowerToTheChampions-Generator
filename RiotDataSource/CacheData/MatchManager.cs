using RiotDataSource.RiotRestAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class MatchManager
    {
        const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private string _rawMatchDataDirectory = "";
        private string _matchDataDTODirectory = "";

        private APIConnection _apiConnection = null;

        public MatchManager(APIConnection apiConnection, string rawMatchDataDirectory, string matchDataDTODirectory)
        {
            _apiConnection = apiConnection;
            _rawMatchDataDirectory = rawMatchDataDirectory;
            _matchDataDTODirectory = matchDataDTODirectory;
        }

        static private void LogProgress(string logMessage)
        {
            Logging.LogManager.LogMessage(logMessage);
        }

        static private string BuildMatchFileName(SeedData.MatchListing matchListing, string matchId)
        {
            return matchListing.region + "-" + matchId + ".json";
        }

        public bool DoesRawCachedCopyOfMatchExist(SeedData.MatchListing matchListing, string matchId)
        {
            string[] rawfilePathParts = new string[] { _rawMatchDataDirectory, BuildMatchFileName(matchListing, matchId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);

            return File.Exists(filePath);
        }

        public bool DoesCachedCopyOfMatchDTOExist(SeedData.MatchListing matchListing, string matchId)
        {
            string[] rawfilePathParts = new string[] { _matchDataDTODirectory, BuildMatchFileName(matchListing, matchId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);

            return File.Exists(filePath);
        }

        public void BeautifyCachedCopyOfMatches(List<SeedData.MatchListing> matchListings)
        {
            foreach (SeedData.MatchListing matchListing in matchListings)
            {
                foreach (string matchId in matchListing.MatchIds)
                {
                    if (DoesRawCachedCopyOfMatchExist(matchListing, matchId))
                    {
                        string[] rawfilePathParts = new string[] { _rawMatchDataDirectory, BuildMatchFileName(matchListing, matchId) };
                        string filePath = System.IO.Path.Combine(rawfilePathParts);
                        BeautifyJSONFile(filePath);
                    }

                    if (DoesCachedCopyOfMatchDTOExist(matchListing, matchId))
                    {
                        string[] dtoFilePathParts = new string[] { _matchDataDTODirectory, BuildMatchFileName(matchListing, matchId) };
                        string filePath = System.IO.Path.Combine(dtoFilePathParts);
                        BeautifyJSONFile(filePath);
                    }
                }
            }
        }

        public void BeautifyJSONFile(string filePath)
        {
            StreamReader filestream = new StreamReader(filePath);
            string rawJSON = filestream.ReadToEnd();
            filestream.Close();
            string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(rawJSON);

            FileStream fstream = new FileStream(filePath, FileMode.Create);
            byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
            fstream.Write(data, 0, data.Length);
            fstream.Close();
        }

        public RiotRestAPI.MatchDTO LoadMatch(SeedData.MatchListing listing, string matchId)
        {
            RiotRestAPI.MatchDTO match = null;

            if (DoesRawCachedCopyOfMatchExist(listing, matchId))
            {
                match = LoadMatchFromRawFile(listing, matchId);
            }
            else
            {
                string rawResponse = "";
                match = LoadMatchFromAPI(listing, matchId, ref rawResponse);

                // Save the raw response file to speed up future queries
                string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(rawResponse);

                string[] rawfilePathParts = new string[] { _rawMatchDataDirectory, listing.region + "-" + matchId + ".json" };
                string filePath = System.IO.Path.Combine(rawfilePathParts);
                FileStream fstream = new FileStream(filePath, FileMode.Create);
                byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
                fstream.Write(data, 0, data.Length);
                fstream.Close();
            }

            return match;
        }

        protected RiotRestAPI.MatchDTO LoadMatchFromRawFile(SeedData.MatchListing matchListing, string matchId)
        {
            RiotRestAPI.MatchDTO match = null;

            string[] rawfilePathParts = new string[] { _rawMatchDataDirectory, BuildMatchFileName(matchListing, matchId) };
            string filePath = System.IO.Path.Combine(rawfilePathParts);
            FileStream fstream = new FileStream(filePath, FileMode.Open);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RiotRestAPI.MatchDTO));
            object objResponse = null;
            try
            {
                objResponse = jsonSerializer.ReadObject(fstream);
            }
            catch (System.Xml.XmlException ex)
            {
                LogProgress("XML Exception while parsing match response: " + matchListing.region + "-" + matchId + " - " + ex.Message);
            }
            catch (Exception ex)
            {
                LogProgress("Generic Exception while parsing match response: " + matchListing.region + "-" + matchId + " - " + ex.Message);
            }

            fstream.Close();
            
            if (objResponse == null)
            {
                LogProgress("Failed to load match " + matchListing.region + "-" + matchId + " from cached data. Deleting file.");
                File.Delete(filePath);
            }
            else
            {
                match = (RiotRestAPI.MatchDTO)Convert.ChangeType(objResponse, typeof(RiotRestAPI.MatchDTO));
                LogProgress("Loaded match " + matchListing.region + "-" + matchId + " from cached data.");
            }

            return match;
        }

        protected RiotRestAPI.MatchDTO LoadMatchFromAPI(SeedData.MatchListing listing, string matchId, ref string rawResponse)
        {
            RiotRestAPI.MatchDTO match = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/" + listing.region + "/v2.2/match/" + matchId;

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                queryParams["includeTimeline"] = "true";
                match = _apiConnection.Get<RiotRestAPI.MatchDTO>(resource, queryParams,
                                                                 ref rateLimitHit, ref rawResponse);
                if (match != null)
                {
                    LogProgress("Loaded match " + listing.region + "-" + matchId + " from the API.");
                }
                else if (rateLimitHit)
                {
                    LogProgress("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogProgress("Unable to load match: " + listing.region + " - " + matchId);
                }
            }

            return match;
        }

        public bool CacheMatchesFromAPI(List<SeedData.MatchListing> listings, CancellationToken cancelationToken)
        {
            foreach (SeedData.MatchListing listing in listings)
            {
                foreach (string matchId in listing.MatchIds)
                {
                    if (cancelationToken.IsCancellationRequested)
                    {
                        return true;
                    }

                    RiotRestAPI.MatchDTO match = LoadMatch(listing, matchId);

                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RiotRestAPI.MatchDTO));
                    MemoryStream mstream = new MemoryStream();
                    ser.WriteObject(mstream, match);
                    string uglyJSON = Encoding.Default.GetString(mstream.ToArray());
                    string prettyJSON = JsonPrettyPrinterPlus.PrettyPrinterExtensions.PrettyPrintJson(uglyJSON);

                    string[] filePathParts = new string[] { _matchDataDTODirectory, BuildMatchFileName(listing, matchId) };
                    string filePath = System.IO.Path.Combine(filePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    byte[] data = Encoding.ASCII.GetBytes(prettyJSON);
                    fstream.Write(data, 0, data.Length);
                    fstream.Close();
                }
            }
            return false;
        }
    }
}
