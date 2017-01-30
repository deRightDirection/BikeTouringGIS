using BicycleTripsPreparationApp;
using BikeTouringGIS.Controls;
using BikeTouringGIS.Messenges;
using BikeTouringGISLibrary;
using Esri.ArcGISRuntime.Layers;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISViewModel : BikeTouringGISBaseViewModel
    {
        private List<string> _loadedFiles = new List<string>();
        private int _splitLength = 100;

        public RelayCommand<BikeTouringGISMapViewModel> OpenGPXFileCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> FlipDirectionCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> SplitRouteCommand { get; private set; }

        public BikeTouringGISViewModel()
        {
            OpenGPXFileCommand = new RelayCommand<BikeTouringGISMapViewModel>(OpenGPXFile);
            FlipDirectionCommand = new RelayCommand<BikeTouringGISLayer>(FlipDirection);
            SplitRouteCommand = new RelayCommand<BikeTouringGISLayer>(SplitRoute);
        }

        private void SplitRoute(BikeTouringGISLayer obj)
        {
//            obj.SplitRoute(SplitLength);
        }

        public int SplitLength
        {
            get { return _splitLength; }
            set { Set(ref _splitLength, value); }
        }

        private void FlipDirection(BikeTouringGISLayer obj)
        {
            obj.FlipDirection();
        }

        private async void OpenGPXFile(BikeTouringGISMapViewModel mapViewModel)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    if(_loadedFiles.Contains(file))
                    {
                        continue;
                    }
                    var gpxFileInformation = new GpxFileReader().LoadFile(file);
                    foreach (var track in gpxFileInformation.Tracks)
                    {
                        StringBuilder textBuilder = new StringBuilder();
                        textBuilder.AppendLine($"Track {track.Name} is defined as track and not as route");
                        textBuilder.AppendLine();
                        textBuilder.AppendLine("routes are used by navigation-devices");
                        textBuilder.AppendLine("tracks are to register where you have been");
                        textBuilder.AppendLine();
                        textBuilder.AppendLine("Do you want to convert it to a route?");
                        var convertTrack = await ConvertTrackToRoute(textBuilder.ToString());
                        if (convertTrack)
                        {
                            track.ConvertTrackToRoute();
                        }
                    }
                    _loadedFiles.Add(file);
                    gpxFileInformation.CreateGeometries();
                    var layer = new BikeTouringGISLayer(file, gpxFileInformation.AllRoutes);
                    mapViewModel.AddRoutes(layer);
                }
            }
        }

        private async Task<bool> ConvertTrackToRoute(string text)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            var result = await window.ShowMessageAsync("Convert track to route", text, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                return true;
            }
            return false;
        }

        public string VersionInformation
        {
            get
            {
                var versionApp = typeof(App).Assembly.GetName().Version;
                return string.Format("version {0}.{1}.{2}", versionApp.Major, versionApp.Minor, versionApp.Build);
            }
        }
    }
}