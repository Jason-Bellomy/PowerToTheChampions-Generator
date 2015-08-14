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
        private bool _cacheMatchJson = false;
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            string applicationPath = Directory.GetCurrentDirectory();
            string[] pathsToSeedData = new string[] { applicationPath, "..", "..", "..", "AP_ITEM_DATASET" };
            _RootSeedDataFileDir_ = System.IO.Path.Combine(pathsToSeedData);

            string[] pathsToCacheData = new string[] { applicationPath, "..", "..", "..", "CachedData" };
            _RootDataCacheFileDir_ = System.IO.Path.Combine(pathsToCacheData);
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
            if (_matchIdFiles.Count == 0)
            {
                log = "No match ID listings to use for match IDs.";
                return;
            }

            if (_cacheMatchJson)
            {
                log = "Starting to pull and cache matches...";
                string rootMatchDataDirectory = System.IO.Path.Combine(_RootDataCacheFileDir_, "MatchData");
                MatchManager.CacheMatchesFromAPI(_matchIdFiles, rootMatchDataDirectory);
            }
            else
            {
                log = "Pulling matches...";
                MatchManager.ReadMatchesFromAPI(_matchIdFiles);
            }
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheckBoxState(sender as CheckBox);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleCheckBoxState(sender as CheckBox);
        }

        private void HandleCheckBoxState(CheckBox checkbox)
        {
            if (checkbox.IsChecked.HasValue)
            {
                _cacheMatchJson = checkbox.IsChecked.Value;
            }
            else
            {
                _cacheMatchJson = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
