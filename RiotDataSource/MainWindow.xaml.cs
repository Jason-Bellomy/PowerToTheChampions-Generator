using RiotDataSource.CacheData;
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
    public partial class MainWindow : Window, INotifyPropertyChanged, Logging.ILogProcessor
    {
        private string _RootSeedDataFileDir_;
        private string _RootDataCacheFileDir_;

        private RiotRestAPI.APIConfig _apiConfig = null;
        private RiotRestAPI.APIConnection _apiConnection = null;

        private MatchManager _matchManager;
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            Logging.LogManager.AddLogProcessor(this);

            string applicationPath = Directory.GetCurrentDirectory();
            string[] pathsToSeedData = new string[] { applicationPath, "..", "..", "..", "AP_ITEM_DATASET" };
            _RootSeedDataFileDir_ = System.IO.Path.Combine(pathsToSeedData);

            string[] pathsToCacheData = new string[] { applicationPath, "..", "..", "..", "CachedData" };
            _RootDataCacheFileDir_ = System.IO.Path.Combine(pathsToCacheData);

            string[] pathsToAPIConfig = new string[] { applicationPath, "..", "..", "..", "CachedData", "ApiConfig", "apiConfig.json" };
            string pathToAPIConfig = System.IO.Path.Combine(pathsToAPIConfig);
            LoadAPIConfig(pathToAPIConfig);

            _apiConnection = new RiotRestAPI.APIConnection(_apiConfig);

            string rawMatchDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "RawMatchData");
            string matchDataDTODirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchData");
            _matchManager = new MatchManager(_apiConnection, rawMatchDataDirectory, matchDataDTODirectory);
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

        public bool CanLoadMatchListings
        {
            get
            {
                return (_matchListings.Count == 0);
            }
        }

        public bool CanCacheMatchData
        {
            get
            {
                return (_selectedMatchListing != null);
            }
        }

        private List<SeedData.MatchListing> _matchListings = new List<SeedData.MatchListing>();
        public List<SeedData.MatchListing> MatchListings
        {
            get
            {
                return _matchListings;
            }
        }

        private SeedData.MatchListing _selectedMatchListing = null;
        public SeedData.MatchListing SelectedMatchListing
        {
            get
            {
                return _selectedMatchListing;
            }
            set
            {
                _selectedMatchListing = value;
                OnPropertyChanged("SelectedMatchListing");
                OnPropertyChanged("CanCacheMatchData");
            }
        }

        private void LoadAPIConfig(string pathToAPIConfig)
        {
            if (!File.Exists(pathToAPIConfig))
            {
                throw new ApplicationException("No API configuration file found.");
            }

            FileStream fstream = new FileStream(pathToAPIConfig, FileMode.Open);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RiotRestAPI.APIConfig));
            object objResponse = jsonSerializer.ReadObject(fstream);
            fstream.Close();
            _apiConfig = (RiotRestAPI.APIConfig)Convert.ChangeType(objResponse, typeof(RiotRestAPI.APIConfig));

            LogProgress("Loaded API Config: " + _apiConfig.ApiKey);
        }

        private void Update_Match_Cache(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(UpdateMatchCache);
        }

        private void UpdateMatchCache(object state)
        {
            if (_matchListings.Count == 0)
            {
                LogProgress("No match ID listings to use for match IDs.");
                return;
            }

            if (_selectedMatchListing == null)
            {
                LogProgress("No match listing selected to use for match IDs.");
                return;
            }

            LogProgress("Starting to pull and cache matches for " + _selectedMatchListing.DisplayName + ".");
            _matchManager.CacheMatchesFromAPI(new List<SeedData.MatchListing>() { _selectedMatchListing });
            LogProgress("Finished caching matches.");
        }

        private void Beautify_Cached_Responses(object state, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(BeautifyCachedMatches);
        }
        
        private void BeautifyCachedMatches(object state)
        {
            if (_matchListings.Count == 0)
            {
                log = "No match ID listings to use for match IDs.";
                return;
            }

            LogProgress("Starting beautification...");
            _matchManager.BeautifyCachedCopyOfMatches(_matchListings);
            LogProgress("Finished beautification.");
        }

        private void LogProgress(string logMessage)
        {
            if (String.IsNullOrEmpty(log))
            {
                log = logMessage;
            }
            else
            {
                log += "\n" + logMessage;
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                LogScrollViewer.ScrollToEnd();
            }));
        }

        private void Read_Input_Match_File(object sender, RoutedEventArgs e)
        {
            if (_matchListings.Count != 0)
            {
                return;
            }

            _matchListings = SeedData.MatchListingManager.LoadMatchDataFromSourceFiles(_RootSeedDataFileDir_);

            log = "Successfully loaded match ID files.";
        }

        private void Write_Match_DataFiles(object sender, RoutedEventArgs e)
        {
            if (_matchListings.Count == 0)
            {
                log = "No match ID listings to cache.";
                return;
            }

            string rootDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchListings");
            SeedData.MatchListingManager.SaveMatchListingsToDisk(rootDataDirectory, _matchListings);
            
            log = "Successfully saved match ID listings to cache.";
        }

        private void Load_Match_DataFiles(object sender, RoutedEventArgs e)
        {
            LogProgress("Beginning match ID load from cache...");

            string rootDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchListings");
            _matchListings = SeedData.MatchListingManager.LoadMatchListingsFromDisk(rootDataDirectory);

            LogProgress("Successfully loaded match ID listings from cache.");
            OnPropertyChanged("CanLoadMatchListings");
            OnPropertyChanged("MatchListings");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ProcessLog(string logMessage)
        {
            LogProgress(logMessage);
        }
    }
}
