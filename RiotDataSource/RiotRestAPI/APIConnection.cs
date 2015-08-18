using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace RiotDataSource.RiotRestAPI
{
    class APIConnection
    {
        public static string GetBaseURI()
        {
            return "https://na.api.pvp.net/api/lol";
        }

        public static T Get<T>(string resource, Dictionary<string, string> queryParams, 
                               ref bool hitRateLimit, ref string rawResponse)
        {
            hitRateLimit = false;

            string serviceURL = GetBaseURI() + resource;
            serviceURL += "?api_key=e0cde70b-3b01-41ea-8d79-8b528f119fd4";

            if (queryParams != null)
            {
                foreach (var param in queryParams)
                {
                    serviceURL += "&" + param.Key + "=" + param.Value;
                }
            }

            HttpWebRequest webRequest = WebRequest.Create(serviceURL) as HttpWebRequest;
            webRequest.UserAgent = "Other";
            try
            {
                using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                {
                    String responseString;
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        rawResponse = responseString;
                        stream.Close();
                    }

                    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseString)))
                    {
                        // Build the DTO object from the response
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        object objResponse = jsonSerializer.ReadObject(stream);
                        stream.Close();
                        return (T)Convert.ChangeType(objResponse, typeof(T));
                    }
                }
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                hitRateLimit = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return default(T);
        }
    }
}
