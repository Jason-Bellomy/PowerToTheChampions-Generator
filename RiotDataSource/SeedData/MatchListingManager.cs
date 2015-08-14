using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace RiotDataSource.SeedData
{
    class MatchListingManager
    {
        static public List<MatchListing> LoadMatchDataFromSourceFiles(string rootDataFileDir)
        {
            List<SeedData.MatchListing> matchIdFiles = new List<SeedData.MatchListing>();

            IEnumerable<String> versionSubDirectories = System.IO.Directory.EnumerateDirectories(rootDataFileDir);
            foreach (string versionDir in versionSubDirectories)
            {
                string version = System.IO.Path.GetFileName(versionDir);

                IEnumerable<string> modeSubDirectories = System.IO.Directory.EnumerateDirectories(versionDir);
                foreach (string modeDir in modeSubDirectories)
                {
                    string mode = System.IO.Path.GetFileName(modeDir);

                    IEnumerable<string> regionFiles = System.IO.Directory.EnumerateFiles(modeDir);
                    foreach (string regionFile in regionFiles)
                    {
                        string region = System.IO.Path.GetFileNameWithoutExtension(regionFile).ToLower();

                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<double>));
                        object objResponse = jsonSerializer.ReadObject(File.Open(regionFile, FileMode.Open));
                        List<double> listing = (List<double>)Convert.ChangeType(objResponse, typeof(List<double>));

                        SeedData.MatchListing matchListingFile = new SeedData.MatchListing();
                        matchListingFile.mode = mode;
                        matchListingFile.region = region;
                        matchListingFile.version = version;
                        foreach (double entry in listing)
                        {
                            matchListingFile.MatchIds.Add(entry.ToString());
                        }
                        matchIdFiles.Add(matchListingFile);
                    }
                }
            }

            return matchIdFiles;
        }

        static public void SaveMatchListingsToDisk(string rootDirectory, List<MatchListing> matchListings)
        {
            foreach (MatchListing listing in matchListings)
            {
                string path = Path.Combine(rootDirectory,
                                           listing.region + "_" + listing.mode + "_" + listing.version + ".json");
                FileStream fstream = new FileStream(path, FileMode.Create);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MatchListing));
                ser.WriteObject(fstream, listing);
                fstream.Close();
            }
        }
        
        static public List<MatchListing> LoadMatchListingsFromDisk(string rootDirectory)
        {
            List<MatchListing> matchListings = new List<MatchListing>();

            IEnumerable<string> files = Directory.EnumerateFiles(rootDirectory);
            foreach (string fileName in files)
            {
                FileStream fstream = new FileStream(fileName, FileMode.Open);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MatchListing));
                object rawListing = ser.ReadObject(fstream);
                matchListings.Add((MatchListing)rawListing);
                fstream.Close();
            }

            return matchListings;
        }
    }
}
