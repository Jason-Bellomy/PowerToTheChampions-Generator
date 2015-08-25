using RiotDataSource.Logging;
using RiotDataSource.RiotRestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.CacheData
{
    class VersionManager
    {
         const int RATE_LIMIT_WAIT_IN_MS = 5000;

        private APIConnection _apiConnection = null;

        public VersionManager(APIConnection apiConnection)
        {
            _apiConnection = apiConnection;
        }

        public List<string> LoadVersions(string region)
        {
            List<string> versions = null;

            bool rateLimitHit = true;
            while (rateLimitHit)
            {
                string resource = "/static-data/" + region + "/v1.2/versions";

                Dictionary<string, string> queryParams = new Dictionary<string, string>();
                string rawResponse = "";
                versions = _apiConnection.Get<List<string>>(resource, queryParams,
                                                                         ref rateLimitHit, ref rawResponse);
                if (versions != null)
                {
                    LogManager.LogMessage("Loaded versions for " + region + " from the API.");
                }
                else if (rateLimitHit)
                {
                    LogManager.LogMessage("Hit rate limit. Waiting to retry.");
                    System.Threading.Thread.Sleep(RATE_LIMIT_WAIT_IN_MS);
                }
                else
                {
                    LogManager.LogMessage("Unable to load versions for region: " + region + ".");
                }
            }

            return versions;
        }
    }
}
