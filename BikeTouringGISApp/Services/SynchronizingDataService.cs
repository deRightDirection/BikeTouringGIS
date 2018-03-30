using BikeTouringGISApp.Library.Comparers;
using BikeTouringGISApp.Library.Enumerations;
using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using BikeTouringGISApp.Repositories;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;
using WinUX.CloudServices.OneDrive;

namespace BikeTouringGISApp.Services
{
    internal class SynchronizingDataService
    {
        private IRepository<LogBook> _logBooksRepository;
        private IRepository<Log> _logsRepository;
//        private OneDriveRepository _oneDriveRepository;

        public SynchronizingDataService()
        {
            var oneDrive = new OneDriveService("00000000441DB51C", new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite", "onedrive.appfolder" });
            var result = await oneDrive.Connect();
            if (result.IsConnected)
            {
                if (Container == null) { Container = new ServiceContainer(); }
                var folder = await oneDrive.GetAppRootFolder();
                var activitiesRepository = new List<Activity>();
                var settingsRepository = new OneDriveRepository<ApplicationSettings>("settings.json", folder);
                var gearRepository = new OneDriveRepository<OneDriveGear>("gear.json", folder);
                var folderRepository = new OneDriveRepository<Activity>("activities.json", folder);

                //           _oneDriveRepository = new OneDriveRepository();
                _logBooksRepository = SimpleIoc.Default.GetInstance<IRepository<LogBook>>();
            _logsRepository = SimpleIoc.Default.GetInstance<IRepository<Log>>();
        }

        public void SaveToLocal(LogBook logbook)
        {
            _logBooksRepository.AddEntity(logbook);
        }

        public void SaveToLocal(Log log)
        {
            _logsRepository.AddEntity(log);
        }

        public void SaveToOneDrive(LogBook logbook)
        {
            // TODO: fixen
//            _oneDriveRepository.SaveOrUpdateLogBook(logbook);
        }

        public void SaveToOneDrive(Log log)
        {
            // TODO: fixen
//            _oneDriveRepository.SaveOrUpdateLog(log);
        }

        public async Task SynchronizeLogBooks()
        {
            await _logBooksRepository.LoadData();
            // TODO: fixen
//            var itemsFromOneDrive = await _oneDriveRepository.GetLogBooks();
//            var itemsLocal = _logBooksRepository.Entities;
//            SyncEntities<LogBook>(itemsLocal, itemsFromOneDrive, SaveToOneDrive, SaveToLocal);
        }

        public async Task SynchronizeLogs()
        {
            await _logsRepository.LoadData();
            // TODO: fixen
//            var itemsFromOneDrive = await _oneDriveRepository.GetLogs();
//            var itemsLocal = _logsRepository.Entities;
//            SyncEntities<Log>(itemsLocal, itemsFromOneDrive, SaveToOneDrive, SaveToLocal);
        }

        private async Task SyncEntities<T>(IEnumerable<T> localItems, IEnumerable<T> oneDriveItems, Action<T> saveToOneDrive, Action<T> saveToLocal) where T : IEntity<T>
        {
            var comparer = new EntityComparer<T>();
            // vind de verschillen
            var difference = localItems.Except(oneDriveItems, comparer).Concat(oneDriveItems.Except(localItems, comparer));
            difference.Where(y => y.Source == RepositorySource.Local).ForEach(x => saveToOneDrive(x));
            difference.Where(y => y.Source == RepositorySource.OneDrive).ForEach(x => saveToLocal(x));
            // items die zowel in OneDrive als lokaal voorkomen
            var equalItems = localItems.Intersect(oneDriveItems, comparer);
            foreach (var equalItem in equalItems)
            {
                T localItem = localItems.Where(x => x.Identifier == equalItem.Identifier).First();
                T oneDriveItem = oneDriveItems.Where(x => x.Identifier == equalItem.Identifier).First();
                var changeStatus = localItem.IsNewerThen(oneDriveItem);
                switch (changeStatus)
                {
                    case 1:
                        saveToOneDrive(localItem);
                        break;

                    case -1:
                        saveToLocal(oneDriveItem);
                        break;
                }
            }
        }
    }

            private IEnumerable<Activity> _strava;
            private StravaService _stravaService;
            private OneDriveRepository<Activity> _repository;
            private CaloriesCalculator _caloriesCalculator;
            private List<Activity> _activities;
            private double _weigth;
            private bool _newStravaActivities, _newObsoleteActivities;

            public async Task PrepareSynchronisation()
            {
                _repository = App.Container.GetInstance<OneDriveRepository<Activity>>();
                _activities = App.Container.GetInstance<List<Activity>>();
                _caloriesCalculator = new CaloriesCalculator();
                _weigth = await ReadWeight();
            }

            public async Task<IEnumerable<Activity>> ReadActivitiesFromOneDrive()
            {
                _activities.Clear();
                var oneDrive = await _repository.GetEntities();
                _activities.AddRange(oneDrive);
                return _activities;
            }

            internal async Task GetActivityIDs()
            {
                _stravaService = await StravaService.GetStravaService();
                _strava = await _stravaService.GetMyActivityDataAsync(new DateTime(1979, 10, 24));
            }

            internal IEnumerable<Activity> CountMissingActiviesFromStrava()
            {
                var comparer = new ActivityComparer();
                var result = _strava.Except(_activities, comparer);
                _newStravaActivities = result.Count() != 0;
                return result;
            }

            internal async Task AddActivityToOneDrive(int activityId)
            {
                var completeActivity = await _stravaService.GetActivityDataAsync(activityId);
                if (completeActivity == null)
                {
                    return;
                }
                var calories = completeActivity.Calories;
                if (calories == 0)
                {
                    calories = _caloriesCalculator.CalculateEnergyUse(completeActivity, _weigth);
                    completeActivity.Calories = calories;
                }
                _activities.Add(completeActivity);
            }

            internal async Task FinishSynchronisation()
            {
                if (_newStravaActivities || _newObsoleteActivities)
                {
                    _repository.ClearEntities();
                    bool isSavedSuccesfully = false;
                    while (!isSavedSuccesfully)
                    {
                        try
                        {
                            isSavedSuccesfully = await _repository.AddEntities(_activities);
                            if (!isSavedSuccesfully)
                            {
                                await Task.Delay(2500);
                            }
                        }
                        catch (Exception e) { }
                    }
                }
            }

            private async Task<double> ReadWeight()
            {
                var settings = App.Container.GetInstance<OneDriveRepository<ApplicationSettings>>();
                var configurations = await settings.GetEntities();
                var config = configurations.FirstOrDefault();
                return config.Weight;
            }

            public IEnumerable<Activity> Activities
            {
                get
                {
                    return _activities;
                }
            }

            internal void RemoveObsoleteActivities()
            {
                var comparer = new ActivityComparer();
                var itemsToRemove = _activities.Except(_strava, comparer).ToList();
                itemsToRemove.ForEach(x => _activities.Remove(x));
                _newObsoleteActivities = itemsToRemove.Count != 0;
            }
        }
    }
}