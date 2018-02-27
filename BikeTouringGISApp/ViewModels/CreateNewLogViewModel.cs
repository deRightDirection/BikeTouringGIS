using BikeTouringGISApp.Library.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Text;
using WinUX.CloudServices.Facebook;
using System.Linq;
using BikeTouringGISApp.Library.Enumerations;
using BikeTouringGISApp.Services;
using System.Threading.Tasks;
using BikeTouringGISApp.Views;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using WinUX.Collections.ObjectModel;
using BikeTouringGISApp.Library.Interfaces;
using WinUX;

namespace BikeTouringGISApp.ViewModels
{
    public class CreateNewLogViewModel : BikeTouringGISBaseViewModel
    {
        // public RelayCommand<ITextDocument> LoadTextCommand { get; private set; }
        private double _latitude, _longitude;

        // private FacebookClient _facebook; private ObservableCollection<FacebookPage>
        // _facebookPages; private string _imageSource;
        private Log _log;

        private IRepository<LogBook> _logBookRepository;
        private ObservableItemCollection<LogBook> _logBooks;
        private int _selectedLogBookIndex;
        // private bool _postToUserFeed; private IEnumerable<FacebookPage> _selectedPages;

        public CreateNewLogViewModel()
        {
            _logBookRepository = SimpleIoc.Default.GetInstance<IRepository<LogBook>>();
            LoadLogBooks();
            // PostToFacebookCommand = new RelayCommand(Post); SelectionChangedCommand = new RelayCommand<IList<object>>(SetSelectedPages);
            SaveTextCommand = new RelayCommand<ITextDocument>(SaveText);
            UpdateGPSCommand = new RelayCommand(UpdateGPS);
            // LoadTextCommand = new RelayCommand<ITextDocument>(LoadText);
            SaveLogCommand = new RelayCommand(SaveLog);
            _selectedLogBookIndex = -1;
            // _facebook = SimpleIoc.Default.GetInstance<FacebookClient>();
            _log = new Log();
        }

        public double Latitude
        {
            get { return _latitude; }
            set { Set(() => Latitude, ref _latitude, value); }
        }

        public Log Log
        {
            get { return _log; }
            set
            {
                Set(() => Log, ref _log, value);
                if (_log.LogBook != null)
                {
                    var foundLogBook = _logBookRepository.Entities.Where(x => x.Identifier.Equals(_log.LogBook)).FirstOrDefault();
                    SelectedLogBookIndex = _logBookRepository.Entities.ToList().IndexOf(foundLogBook);
                }
            }
        }

        public ObservableItemCollection<LogBook> LogBooks
        {
            get { return _logBooks; }
            set { Set(() => LogBooks, ref _logBooks, value); }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { Set(() => Longitude, ref _longitude, value); }
        }

        public RichEditBox RichTextBox { get; internal set; }

        public RelayCommand SaveLogCommand { get; private set; }

        public RelayCommand<ITextDocument> SaveTextCommand { get; private set; }

        public int SelectedLogBookIndex
        {
            get { return _selectedLogBookIndex; }
            set { Set(() => SelectedLogBookIndex, ref _selectedLogBookIndex, value); }
        }

        public RelayCommand UpdateGPSCommand { get; private set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                await LoadLogBooks();
                Log = (Log)parameter;
                var story = Log.Stories.Where(x => x.Language == LogStoryLanguage.Dutch).FirstOrDefault();
                if (story != null)
                {
                    RichTextBox.Document.SetText(TextSetOptions.None, story.Text);
                }
                Latitude = Log.Latitude;
                Longitude = Log.Longitude;
            }
        }

        private async Task LoadLogBooks()
        {
            await _logBookRepository.LoadData();
            var items = new List<LogBook>();
            LogBooks = new ObservableItemCollection<LogBook>(_logBookRepository.Entities);
        }

        private async void SaveLog()
        {
            try
            {
                if (string.IsNullOrEmpty(_log.FileName))
                {
                    _log.SetFileName();
                }
                _log.Latitude = Latitude;
                _log.Longitude = Longitude;
                _log.LastModificationDate = DateTime.Now;
                var item = LogBooks.ElementAt(SelectedLogBookIndex);
                _log.LogBook = item.Identifier;
                await FileService.Instance.SaveLog(_log);
            }
            catch
            { }
        }

        private void SaveText(ITextDocument document)
        {
            string text;
            document.GetText(TextGetOptions.AdjustCrlf, out text);
            _log?.AddLogStory(new LogStory() { Text = text });
            if (!string.IsNullOrEmpty(text))
            {
                SaveLog();
            }
        }

        private async Task SetLocation()
        {
            Busy.SetBusy(true, "get gps location");
            var getAccessStatusFromSettings = SettingsService.Instance.AllowUseGPSDevice;
            if (getAccessStatusFromSettings == GeolocationAccessStatus.Unspecified)
            {
                SettingsService.Instance.AllowUseGPSDevice = await Geolocator.RequestAccessAsync();
                getAccessStatusFromSettings = SettingsService.Instance.AllowUseGPSDevice;
            }
            if (getAccessStatusFromSettings == GeolocationAccessStatus.Allowed)
            {
                Geolocator locator = new Geolocator();
                try
                {
                    var position = await locator.GetGeopositionAsync();
                    Latitude = position.Coordinate.Latitude;
                    Longitude = position.Coordinate.Longitude;
                }
                catch
                {
                    Latitude = 0;
                    Longitude = 0;
                }
            }
            Busy.SetBusy(false);
        }

        private async void UpdateGPS()
        {
            await SetLocation();
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
        */
        // public RelayCommand PostToFacebookCommand { get; private set; }

        /*
    public bool PostToUserFeed
    {
        get { return _postToUserFeed; }
        set { Set(() => PostToUserFeed, ref _postToUserFeed, value); }
    }
    */
        // public RelayCommand<IList<object>> SelectionChangedCommand { get; private set; }

        // private void LoadText(ITextDocument document) { document.SetText(TextSetOptions.None,
        // _log.Stories.Where(x => x.Language == LogStoryLanguage.Dutch).FirstOrDefault()?.Text); }

        /*

private async void Post()
{
FileOpenPicker openPicker = new FileOpenPicker
{
ViewMode = PickerViewMode.Thumbnail,
SuggestedStartLocation = PickerLocationId.PicturesLibrary
};
openPicker.FileTypeFilter.Add(".jpg");
openPicker.FileTypeFilter.Add(".jpeg");
openPicker.FileTypeFilter.Add(".png");

var files = await openPicker.PickMultipleFilesAsync();
var pictureIds = new List<string>();
var feedId = _facebook.FacebookService.Provider.User.Id;
foreach (var file in files)
{
IRandomAccessStreamWithContentType randomAccessStream = await file.OpenReadAsync();
var picture = await _facebook.FacebookService.PostPictureToFeedAsync(feedId, false, "Mannus2, dit is een testberichtje dus ben aan het kijken hoe werkt", file.DisplayName, randomAccessStream);
pictureIds.Add(picture.Id);
}

var image = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(ImageSource));
IRandomAccessStreamWithContentType randomAccessStream = await image.OpenReadAsync();

string y = await service.PostPictureToFeedAsync(, false, "test", "test2", randomAccessStream);
string y = await service.PostPictureToFeedAsync("Mannus", "Etten", randomAccessStream);
var place = new facebook.FacebookPlace
{
Location = new facebook.FacebookLocation()
{
    Latitude = 53.21997,
    Longitude = 6.57064
}
};
// var places = await _facebook.FacebookService.GetPlacesByCoordinatesAsync(Latitude, Longitude); var
// place = places.First().Id; bool x = await _facebook.FacebookService.PostToFeedAsync(message:
// "Mannus", place: place);
}
*/
        /*
    private async void SetInitialData()
    {
        FileSavePicker picker = new FileSavePicker();
        picker.SuggestedFileName = "test.json";
        picker.
        Busy.SetBusy(true, "loading OneDrive...");
        OneDriveService.Instance.Initialize("00000000441DB51C", AccountProviderType.Msa, OneDriveScopes.OfflineAccess | OneDriveScopes.ReadWrite | OneDriveScopes.AppFolder);
        await OneDriveService.Instance.LoginAsync();
        var folder = await OneDriveService.Instance.RootFolderAsync();
        var path = folder.Path;
        Busy.SetBusy(false);
        Busy.SetBusy(true, "loading Facebook data...");
        var pages = await _facebook.GetPagesAsync();
        FacebookPages = new ObservableCollection<FacebookPage>(pages);
        var userPicture = await _facebook.FacebookService.GetUserPictureInfoAsync();
        ImageSource = userPicture.Url;
        Busy.SetBusy(false);
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