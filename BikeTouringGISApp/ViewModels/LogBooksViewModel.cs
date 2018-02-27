using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using BikeTouringGISApp.Repositories;
using BikeTouringGISApp.Services;
using BikeTouringGISApp.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;
using WinUX.Collections.ObjectModel;

namespace BikeTouringGISApp.ViewModels
{
    public class LogBooksViewModel : BikeTouringGISBaseViewModel
    {
        private IRepository<LogBook> _logBookRepository;
        private ObservableItemCollection<LogBookViewModel> _logBooks;

        public LogBooksViewModel()
        {
            NewLogBookCommand = new RelayCommand(NewLogBook);
            SyncLogBooksCommand = new RelayCommand(SyncLogBooks);
            EditLogBookCommand = new RelayCommand<LogBookViewModel>(EditLogBook);
            CancelLogBookCommand = new RelayCommand<LogBookViewModel>(CancelLogBook);
            DeleteLogBookCommand = new RelayCommand<LogBookViewModel>(DeleteLogBook);
            SaveLogBookCommand = new RelayCommand<LogBookViewModel>(SaveLogBook);
            _logBookRepository = SimpleIoc.Default.GetInstance<IRepository<LogBook>>();
            LoadLogBooks();
        }

        public RelayCommand<LogBookViewModel> CancelLogBookCommand { get; private set; }

        public RelayCommand<LogBookViewModel> DeleteLogBookCommand { get; private set; }

        public RelayCommand<LogBookViewModel> EditLogBookCommand { get; private set; }

        public ObservableItemCollection<LogBookViewModel> LogBooks
        {
            get { return _logBooks; }
            set { Set(() => LogBooks, ref _logBooks, value); }
        }

        public RelayCommand NewLogBookCommand { get; private set; }

        public RelayCommand<LogBookViewModel> SaveLogBookCommand { get; private set; }

        public RelayCommand SyncLogBooksCommand { get; private set; }

        private void CancelLogBook(LogBookViewModel obj)
        {
            obj.DataFormIsInReadOnlyMode = true;
            LoadLogBooks();
        }

        private async void DeleteLogBook(LogBookViewModel obj)
        {
            LogBooks.Remove(obj);
            await _logBookRepository.DeleteEntity(obj.LogBook);
        }

        private void EditLogBook(LogBookViewModel obj)
        {
            obj.DataFormIsInReadOnlyMode = false;
        }

        private async void LoadLogBooks()
        {
            await _logBookRepository.LoadData();
            var items = new List<LogBookViewModel>();
            _logBookRepository.Entities.ForEach(x => items.Add(new LogBookViewModel() { LogBook = x }));
            LogBooks = new ObservableItemCollection<LogBookViewModel>(items);
        }

        private void NewLogBook()
        {
            var logbook = new LogBookViewModel() { LogBook = new LogBook() { Name = "new" } };
            LogBooks.Add(logbook);
        }

        private void SaveLogBook(LogBookViewModel obj)
        {
            obj.LogBook.LastModificationDate = DateTime.Now;
            _logBookRepository.AddEntity(obj.LogBook);
        }

        private async void SyncLogBooks()
        {
            Busy.SetBusy(true, "sync logbooks with OneDrive");
            var syncService = new SynchronizingDataService();
            await syncService.SynchronizeLogBooks();
            Busy.SetBusy(false);
        }
    }
}