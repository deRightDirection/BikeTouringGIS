using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using BikeTouringGISApp.Services;
using BikeTouringGISApp.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using WinUX;
using WinUX.CloudServices.Facebook;
using WinUX.Collections.ObjectModel;

namespace BikeTouringGISApp.ViewModels
{
    public class LogsViewModel : BikeTouringGISBaseViewModel
    {
        private ObservableCollection<Log> _logs;
        //private FacebookClient _facebook;
        //private ObservableCollection<FacebookPage> _facebookPages;
        //private string _imageSource;
        //private bool _postToUserFeed;
        //private IEnumerable<FacebookPage> _selectedPages;

        public LogsViewModel()
        {
            EditLogCommand = new RelayCommand<Log>(EditLog);
            SyncLogsCommand = new RelayCommand(SyncLogs);
            SetInitialData();
            /*
            ImageSource = "ms-appx:///Assets/SplashScreen.png";
            PostToFacebookCommand = new RelayCommand(Post);
            SelectionChangedCommand = new RelayCommand<IList<object>>(SetSelectedPages);
            // _facebook = SimpleIoc.Default.GetInstance<FacebookClient>();
            */
        }

        public RelayCommand<Log> EditLogCommand { get; private set; }

        public ObservableCollection<Log> Logs
        {
            get { return _logs; }
            set { Set(() => Logs, ref _logs, value); }
        }

        public string LogsDirectory
        {
            get { return FileService.Instance.Path; }
        }

        public RelayCommand SyncLogsCommand { get; private set; }

        private void EditLog(Log log)
        {
            NavigationService.Navigate(typeof(CreateNewLog), log);
        }

        private async void SetInitialData()
        {
            Busy.SetBusy(true, "Loading logs...");
            var logs = await FileService.Instance.GetLogs();
            logs = logs.OrderByDescending(x => x.Date);
            Logs = new ObservableCollection<Log>(logs);

            RaisePropertyChanged("LogsDirectory");

            Busy.SetBusy(false);

            /*
            var pages = await _facebook.GetPagesAsync();
            FacebookPages = new ObservableCollection<FacebookPage>(pages);
            var userPicture = await _facebook.FacebookService.GetUserPictureInfoAsync();
            */
        }

        private void SyncLogs()
        {
            Busy.SetBusy(true, "sync logs with OneDrive");
            Busy.SetBusy(false);
        }

        /*
        public ObservableCollection<FacebookPage> FacebookPages
        {
            get { return _facebookPages; }
            set { Set(() => FacebookPages, ref _facebookPages, value); }
        }

        public string ImageSource
        {
            get { return _imageSource; }
            set { Set(() => ImageSource, ref _imageSource, value); }
        }

        public RelayCommand PostToFacebookCommand { get; private set; }

        public RelayCommand<IList<object>> SelectionChangedCommand { get; private set; }

        private async void Post()
        {
            var image = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(ImageSource));
            IRandomAccessStreamWithContentType randomAccessStream = await image.OpenReadAsync();

            string y = await service.PostPictureToFeedAsync(, false, "test", "test2", randomAccessStream);
            string y = await service.PostPictureToFeedAsync("Mannus", "Etten", randomAccessStream);

            // bool x = await service.PostToFeedAsync("Mannus", "Etten", "Orsi",
            // "http://www.basketbalnieuws.nl", ImageSource);
        }
        */
        /*
        private void SetSelectedPages(IList<object> obj)
        {
            var x = obj;
            // _selectedPages = obj;
        }
        */
    }
}