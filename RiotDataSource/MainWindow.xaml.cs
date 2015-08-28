using RiotDataSource.CacheData;
using RiotDataSource.Logging;
using RiotDataSource.RiotRestAPI;
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

        private CacheDirectoryManager _cacheDirectoryManager = null;

        private MatchManager _matchManager = null;

        private VersionManager _versionManager = null;
        private ItemManager _itemManager = null;
        private ChampionManager _championManager = null;

        private CancellationTokenSource _matchCacheCancelationSource = null;
        private CancellationTokenSource _itemCacheCancelationSource = null;
        private CancellationTokenSource _champCacheCancelationSource = null;
        
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
            _cacheDirectoryManager = new CacheDirectoryManager(_RootDataCacheFileDir_);

            LogManager.LogMessage("Verifying cache folder structure...");
            _cacheDirectoryManager.VerifyFolderStructure();
            LogManager.LogMessage("Cache folder structure setup.");

            LoadAPIConfig(_cacheDirectoryManager.APIConfigFilePath);
            _apiConnection = new RiotRestAPI.APIConnection(_apiConfig);

            _versionManager = new VersionManager(_apiConnection);

            _matchManager = new MatchManager(_apiConnection, _cacheDirectoryManager.MatchDataFolderPath);
            _itemManager = new ItemManager(_apiConnection, _cacheDirectoryManager.ItemDataFolderPath);
            _championManager = new ChampionManager(_apiConnection, _cacheDirectoryManager.ChampionDataFolderPath);
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

        public bool CanLoadVersions
        {
            get
            {
                return (_selectedMatchListing != null);
            }
        }

        public bool CanCacheMatchData
        {
            get
            {
                return (_selectedMatchListing != null);
            }
        }

        public bool CanCacheItemData
        {
            get
            {
                return (_selectedMatchListing != null && _selectedVersion != null);
            }
        }

        public bool CanCacheChampData
        {
            get
            {
                return (_selectedMatchListing != null && _selectedVersion != null);
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
                OnPropertyChanged("CanLoadVersions");
                OnPropertyChanged("CanCacheMatchData");
                OnPropertyChanged("CanCacheItemData");
                OnPropertyChanged("CanCacheChampData");

                Versions = null;
            }
        }

        private List<string> _versions = null;
        public List<string> Versions
        {
            get
            {
                return _versions;
            }
            set
            {
                _versions = value;

                OnPropertyChanged("Versions");
            }
        }

        private String _selectedVersion = null;
        public String SelectedVersion
        {
            get
            {
                return _selectedVersion;
            }
            set
            {
                if (value != _selectedVersion)
                {
                    _selectedVersion = value;

                    OnPropertyChanged("CanCacheItemData");
                    OnPropertyChanged("CanCacheChampData");

                    VersionedItems = null;
                    VersionedChampions = null;
                }
            }
        }

        private List<ItemDTO> _versionedItems = null;
        public List<ItemDTO> VersionedItems
        {
            get
            {
                return _versionedItems;
            }
            set
            {
                _versionedItems = value;
                OnPropertyChanged("VersionedItems");
            }
        }

        private List<ChampionDTO> _versionedChampions = null;
        public List<ChampionDTO> VersionedChampions
        {
            get
            {
                return _versionedChampions;
            }
            set
            {
                _versionedChampions = value;
                OnPropertyChanged("VersionedChampions");
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

        private void Load_Versions_For_Region(object sender, RoutedEventArgs e)
        {
            LogManager.LogMessage("Starting version load...");
            Versions = _versionManager.LoadVersions(_selectedMatchListing.region);
            LogManager.LogMessage("Finished version load.");
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

            _matchCacheCancelationSource = new CancellationTokenSource();
            _matchManager.CacheMatchesFromAPI(new List<SeedData.MatchListing>() { _selectedMatchListing }, _matchCacheCancelationSource.Token);
            _matchCacheCancelationSource = null;

            LogProgress("Finished caching matches.");
        }

        private void Update_Item_Cache(object state, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(UpdateItemCache);
        }

        private void UpdateItemCache(object state)
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

            LogManager.LogMessage("Starting to pull and cache items...");
            LogManager.LogMessage("- Pulling item ID list for selected region/version...");
            List<int> itemIds = _itemManager.LoadItemIds(_selectedMatchListing.region, _selectedVersion);
            LogManager.LogMessage("- Pulled " + itemIds.Count + " item Ids.");

            LogManager.LogMessage("- Pulling items by id...");
            List<ItemDTO> items = new List<ItemDTO>();
            _itemCacheCancelationSource = new CancellationTokenSource();
            foreach (int itemId in itemIds)
            {
                if (_itemCacheCancelationSource.Token.IsCancellationRequested)
                {
                    break;
                }

                ItemDTO item = _itemManager.LoadItem(_selectedMatchListing.region, _selectedVersion, itemId);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            VersionedItems = items;
            _itemCacheCancelationSource = null;
            LogManager.LogMessage("Finished caching items.");
        }

        private void Update_Champ_Cache(object state, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(UpdateChampCache);
        }

        private void UpdateChampCache(object state)
        {
            if (_matchListings.Count == 0)
            {
                LogManager.LogMessage("No match ID listings to use for match IDs.");
                return;
            }

            if (_selectedMatchListing == null)
            {
                LogManager.LogMessage("No match listing selected to use for match IDs.");
                return;
            }

            if (_selectedVersion == null || _selectedVersion == "")
            {
                LogManager.LogMessage("No version selected to use for querying champions");
                return;
            }

            LogProgress("Starting to pull and cache champions.");
            _champCacheCancelationSource = new CancellationTokenSource();

            List<int> championIds = _championManager.LoadIds(_selectedMatchListing.region, _selectedVersion);
            LogProgress(" - Pulled " + championIds.Count + " champion ids for version " + _selectedVersion);

            List<ChampionDTO> champions = new List<ChampionDTO>();
            foreach (int championId in championIds)
            {
                if (_champCacheCancelationSource.Token.IsCancellationRequested)
                {
                    break;
                }

                ChampionDTO thisChamp = _championManager.LoadChampion(_selectedMatchListing.region,
                                                                      _selectedVersion, championId);

                if (thisChamp != null)
                {
                    champions.Add(thisChamp);
                }
            }
            _champCacheCancelationSource = null;
            VersionedChampions = champions;

            LogProgress("Finished caching champions.");
        }

        private void Stop_Cache_Tasks(object state, RoutedEventArgs e)
        {
            if (_matchCacheCancelationSource != null)
            {
                _matchCacheCancelationSource.Cancel();
            }
            
            if (_itemCacheCancelationSource != null)
            {
                _itemCacheCancelationSource.Cancel();
            }

            if (_champCacheCancelationSource != null)
            {
                _champCacheCancelationSource.Cancel();
            }
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
