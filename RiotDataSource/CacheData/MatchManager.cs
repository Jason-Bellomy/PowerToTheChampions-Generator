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

        static private void LogProgress(string logMessage)
        {
            Console.WriteLine(logMessage);
        }

        static public RiotRestAPI.MatchDTO LoadMatchFromAPI(string region, string matchId)
        {
            RiotRestAPI.MatchDTO match = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/" + region + "/v2.2/match/" + matchId;

                match = RiotRestAPI.APIConnection.Get<RiotRestAPI.MatchDTO>(resource, ref rateLimitHit);
                if (match != null)
                {
                    LogProgress(match.ToString());
                }
                else if (rateLimitHit)
                {
                    LogProgress("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogProgress("Unable to load match: " + region + " - " + matchId);
                }
            }

            return match;
        }

        static public void CacheMatchesFromAPI(List<SeedData.MatchListing> listings, string rootMatchDataDirectory)
        {
            foreach (SeedData.MatchListing listing in listings)
            {
                foreach (string matchId in listing.MatchIds)
                {
                    RiotRestAPI.MatchDTO match = LoadMatchFromAPI(listing.region, matchId);

                    string[] filePathParts = new string[] {rootMatchDataDirectory, listing.region + "-" + matchId + ".json"};
                    string filePath = System.IO.Path.Combine(filePathParts);
                    FileStream fstream = new FileStream(filePath, FileMode.Create);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RiotRestAPI.MatchDTO));
                    ser.WriteObject(fstream, match);
                    fstream.Close();
                }
            }
        }

        static public void ReadMatchesFromAPI(List<SeedData.MatchListing> listings)
        {
            foreach (SeedData.MatchListing listing in listings)
            {
                foreach (string matchId in listing.MatchIds)
                {
                    RiotRestAPI.MatchDTO match = LoadMatchFromAPI(listing.region, matchId);       
                }
            }
        }
    }
}
