﻿using BikeTouringGISApp.Library.Comparers;
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

namespace BikeTouringGISApp.Services
{
    internal class SynchronizingDataService
    {
        private IRepository<LogBook> _logBooksRepository;
        private IRepository<Log> _logsRepository;
        private OneDriveRepository _oneDriveRepository;

        public SynchronizingDataService()
        {
            _oneDriveRepository = new OneDriveRepository();
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
            _oneDriveRepository.SaveOrUpdateLogBook(logbook);
        }

        public void SaveToOneDrive(Log log)
        {
            _oneDriveRepository.SaveOrUpdateLog(log);
        }

        public async Task SynchronizeLogBooks()
        {
            await _logBooksRepository.LoadData();
            var itemsFromOneDrive = await _oneDriveRepository.GetLogBooks();
            var itemsLocal = _logBooksRepository.Entities;
            SyncEntities<LogBook>(itemsLocal, itemsFromOneDrive, SaveToOneDrive, SaveToLocal);
        }

        public async Task SynchronizeLogs()
        {
            await _logsRepository.LoadData();
            var itemsFromOneDrive = await _oneDriveRepository.GetLogs();
            var itemsLocal = _logsRepository.Entities;
            SyncEntities<Log>(itemsLocal, itemsFromOneDrive, SaveToOneDrive, SaveToLocal);
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
}