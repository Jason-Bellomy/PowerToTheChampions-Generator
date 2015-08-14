using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RiotDataSource
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<SeedData.MatchListing> matchIdFiles = new List<SeedData.MatchListing>();
        private int RATE_LIMIT_WAIT_IN_MS = 5000;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private string _log;
        public string log
        {
            private set
            {
                _log = value;
                OnPropertyChanged("log");
            }
            get
            {
                return _log;
            }
        }

        private void Start_Pulling_From_API(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ReadMatchesFromAPI);
        }

        private void ReadMatchesFromAPI(object state)
        {
            foreach (SeedData.MatchListing listing in matchIdFiles)
            {
                foreach (string matchId in listing.MatchIds)
                {
                    bool rateLimitHit = true;
                    while (rateLimitHit)
                    {
                        string resource = "/" + listing.region + "/v2.2/match/" + matchId;

                        RiotRestAPI.MatchDTO match = RiotRestAPI.APIConnection.Get<RiotRestAPI.MatchDTO>(resource, ref rateLimitHit);
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
                            LogProgress("Unable to load match: " + listing.region + " - " + matchId);
                        }
                    }
                }
            }
        }

        private void LogProgress(string logMessage)
        {
            log = logMessage + "\n" + log;
            
        }

        private void Read_Input_Match_File(object sender, RoutedEventArgs e)
        {
            if (matchIdFiles.Count != 0)
            {
                return;
            }

            string rootDataFileDir = "Y:\\documents\\visual studio 2013\\Projects\\RiotDataSource\\RiotDataSource\\bin\\Debug\\..\\..\\..\\AP_ITEM_DATASET\\";

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
                        foreach(double entry in listing)
                        {
                            matchListingFile.MatchIds.Add(entry.ToString());
                        }
                        matchIdFiles.Add(matchListingFile);
                    }
                }
            }

            log = "Successfully loaded match ID files.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
