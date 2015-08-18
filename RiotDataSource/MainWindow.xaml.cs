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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _RootSeedDataFileDir_;
        private string _RootDataCacheFileDir_ = "Y:\\Documents\\PowerToTheChampions-Generator\\DataCache\\";

        private List<SeedData.MatchListing> _matchIdFiles = new List<SeedData.MatchListing>();

        private MatchManager _matchManager;
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            string applicationPath = Directory.GetCurrentDirectory();
            string[] pathsToSeedData = new string[] { applicationPath, "..", "..", "..", "AP_ITEM_DATASET" };
            _RootSeedDataFileDir_ = System.IO.Path.Combine(pathsToSeedData);

            string[] pathsToCacheData = new string[] { applicationPath, "..", "..", "..", "CachedData" };
            _RootDataCacheFileDir_ = System.IO.Path.Combine(pathsToCacheData);

            string rawMatchDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "RawMatchData");
            string matchDataDTODirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchData");
            _matchManager = new MatchManager(rawMatchDataDirectory, matchDataDTODirectory);
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

        private void Update_Match_Cache(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(UpdateMatchCache);
        }

        private void UpdateMatchCache(object state)
        {
            if (_matchIdFiles.Count == 0)
            {
                log = "No match ID listings to use for match IDs.";
                return;
            }

            LogProgress("Starting to pull and cache matches...");
            _matchManager.CacheMatchesFromAPI(_matchIdFiles);
            LogProgress("Finished caching matches.");
        }

        private void Beautify_Cached_Responses(object state, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(BeautifyCachedMatches);
        }
        
        private void BeautifyCachedMatches(object state)
        {
            if (_matchIdFiles.Count == 0)
            {
                log = "No match ID listings to use for match IDs.";
                return;
            }

            LogProgress("Starting beautification...");
            _matchManager.BeautifyCachedCopyOfMatches(_matchIdFiles);
            LogProgress("Finished beautification.");
        }

        private void LogProgress(string logMessage)
        {
            log = logMessage + "\n" + log;
        }

        private void Read_Input_Match_File(object sender, RoutedEventArgs e)
        {
            if (_matchIdFiles.Count != 0)
            {
                return;
            }

            _matchIdFiles = SeedData.MatchListingManager.LoadMatchDataFromSourceFiles(_RootSeedDataFileDir_);

            log = "Successfully loaded match ID files.";
        }

        private void Write_Match_DataFiles(object sender, RoutedEventArgs e)
        {
            if (_matchIdFiles.Count == 0)
            {
                log = "No match ID listings to cache.";
                return;
            }

            string rootDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchListings");
            SeedData.MatchListingManager.SaveMatchListingsToDisk(rootDataDirectory, _matchIdFiles);
            
            log = "Successfully saved match ID listings to cache.";
        }

        private void Load_Match_DataFiles(object sender, RoutedEventArgs e)
        {
            string rootDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchListings");
            _matchIdFiles = SeedData.MatchListingManager.LoadMatchListingsFromDisk(rootDataDirectory);

            log = "Successfully loaded match ID listings from cache.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
