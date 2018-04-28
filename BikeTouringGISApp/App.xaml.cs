using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using WinUX.CloudServices;
using WinUX.CloudServices.OneDrive;
using Template10.Common;
using BikeTouringGISApp.Services;
using WinUX.CloudServices.Facebook;
using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using WinUX.Data;

namespace BikeTouringGISApp
{
    // OneDrive client ID --> 00000000441DB51C

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            await ConnectToOneDrive();
            await FileService.Instance.SetMainFolder();
            SimpleIoc.Default.Register(() => new StorageFileRepository<LogBook>("logbooks.json", FileService.Instance.ApplicationFolder));
            /*
            var fbClient = new FacebookClient();
            await fbClient.LoginAsync("687964081409306");
            SimpleIoc.Default.Register<FacebookClient>(() => fbClient);
            */
            NavigationService.Navigate(typeof(Views.CreateNewLog));
        }

        private async Task ConnectToOneDrive()
        {
            var oneDrive = new OneDriveService("00000000441DB51C", new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite", "onedrive.appfolder" });
            var result = await oneDrive.Connect();
            if (result.IsConnected)
            {
                var folder = await oneDrive.GetAppRootFolder();
                SimpleIoc.Default.Register(() => new OneDriveRepository<LogBook>("logbooks.json", folder));
                SimpleIoc.Default.Register(() => new OneDriveFolderRepository<Log>(folder, "Logs"));
            }
        }
    }
}