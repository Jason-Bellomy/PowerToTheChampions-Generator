using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class MatchManager
    {
        const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private string _rawMatchDataDirectory = "";
        private string _matchDataDTODirectory = "";

        public MatchManager(string rawMatchDataDirectory, string matchDataDTODirectory)
        {
            _rawMatchDataDirectory = rawMatchDataDirectory;
            _matchDataDTODirectory = matchDataDTODirectory;
        }

        static private void LogProgress(string logMessage)
        {
            Console.WriteLine(logMessage);
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
                string[] rawfilePathParts = new string[] { _rawMatchDataDirectory, listing.region + "-" + matchId + ".json" };
                string filePath = System.IO.Path.Combine(rawfilePathParts);
                FileStream fstream = new FileStream(filePath, FileMode.Create);
                byte[] data = Encoding.ASCII.GetBytes(rawResponse);
                fstream.Write(data, 0, data.Length);
                fstream.Close();
            }

            return match;
        }

        protected RiotRestAPI.MatchDTO LoadMatchFromRawFile(SeedData.MatchListing listing, string matchId)
        {
            return null;
        }

        protected RiotRestAPI.MatchDTO LoadMatchFromAPI(SeedData.MatchListing listing, string matchId, ref string rawResponse)
        {
            RiotRestAPI.MatchDTO match = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/" + listing.region + "/v2.2/match/" + matchId;

                match = RiotRestAPI.APIConnection.Get<RiotRestAPI.MatchDTO>(resource, ref rateLimitHit, ref rawResponse);
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

        public void CacheMatchesFromAPI(List<SeedData.MatchListing> listings)
        {
            foreach (SeedData.MatchListing listing in listings)
            {
                foreach (string matchId in listing.MatchIds)
                {
                    RiotRestAPI.MatchDTO match = LoadMatch(listing, matchId);

                    string[] filePathParts = new string[] { _matchDataDTODirectory, BuildMatchFileName(listing, matchId) };
                    string filePath = System.IO.Path.Combine(filePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RiotRestAPI.MatchDTO));
                    ser.WriteObject(fstream, match);
                    fstream.Close();
                }
            }
        }
    }
}
