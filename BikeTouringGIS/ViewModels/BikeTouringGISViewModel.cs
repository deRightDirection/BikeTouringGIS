using BicycleTripsPreparationApp;
using BikeTouringGIS.Controls;
using BikeTouringGIS.Messenges;
using BikeTouringGIS.Models;
using BikeTouringGIS.Services;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using BikeTouringGISLibrary.Services;
using GalaSoft.MvvmLight.Command;
using GPX;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISViewModel : BikeTouringGISBaseViewModel
    {
        private List<string> _loadedFiles = new List<string>();
        private int _splitLength;
        private bool _convertTracksToRoutesAutomatically;

        public RelayCommand<BikeTouringGISMapViewModel> OpenGPXFileCommand { get; private set; }
        public RelayCommand<SplitLayerProperties> ChangeSplitRouteCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> FlipDirectionCommand { get; private set; }
        public RelayCommand<SplitLayerProperties> SplitRouteCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> RemoveSplitRouteCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> SaveSplitRouteCommand { get; private set; }
        public RelayCommand<BikeTouringGISLayer> CenterToLayerCommand { get; private set; }
        public RelayCommand CenterCommand { get; private set; }
        public RelayCommand ZoomInCommand { get; private set; }
        public RelayCommand ZoomOutCommand { get; private set; }

        public BikeTouringGISViewModel()
        {
            OpenGPXFileCommand = new RelayCommand<BikeTouringGISMapViewModel>(OpenGPXFile);
            FlipDirectionCommand = new RelayCommand<BikeTouringGISLayer>(FlipDirection);
            SplitRouteCommand = new RelayCommand<SplitLayerProperties>(SplitRoute);
            ChangeSplitRouteCommand = new RelayCommand<SplitLayerProperties>(SplitRoute, CanReSplitRoute);
            RemoveSplitRouteCommand = new RelayCommand<BikeTouringGISLayer>(RemoveSplitRoute);
            SaveSplitRouteCommand = new RelayCommand<BikeTouringGISLayer>(SaveSplitRoute);
            CenterToLayerCommand = new RelayCommand<BikeTouringGISLayer>(CenterMap);
            CenterCommand = new RelayCommand(() => CenterMap(null));
            ZoomInCommand = new RelayCommand(() => ZoomInOrOutMap(ZoomOption.ZoomIn));
            ZoomOutCommand = new RelayCommand(() => ZoomInOrOutMap(ZoomOption.ZoomOut));
            MessengerInstance.Register<LayerRemovedMessage>(this, LayerRemoved);
        }

        private void ZoomInOrOutMap(ZoomOption zoomOption)
        {
            var message = zoomOption == ZoomOption.ZoomIn ? new ExtentChangedMessage(ExtentChangedReason.ZoomIn) : new ExtentChangedMessage(ExtentChangedReason.ZoomOut);
            MessengerInstance.Send(message);
        }

        private void CenterMap(BikeTouringGISLayer obj)
        {
            var message = obj == null ? new ExtentChangedMessage(ExtentChangedReason.CenterMap) : new ExtentChangedMessage(ExtentChangedReason.CenterLayer) { Extent = obj?.Extent };
            MessengerInstance.Send(message);
        }

        private void SaveSplitRoute(BikeTouringGISLayer obj)
        {
            var baseDirectory = Path.GetDirectoryName(obj.FileName);
            int i = 1;
            foreach (var splitRoute in obj.SplitRoutes)
            {
                var filename = string.Format(@"{0}\{1}_{2}.gpx", baseDirectory, obj.SplitPrefix, i);
                var gpxFile = new GPXFile();
                var gpx = new gpxType();
                var rte = new rteType();
                rte.name = $"{i}_{obj.SplitPrefix}";
                rte.rtept = splitRoute.Points.ToArray();
                gpx.rte = new List<rteType>() { rte }.ToArray();
                gpxFile.Save(filename, gpx);
                i++;
            }
        }

        private void LayerRemoved(LayerRemovedMessage obj)
        {
            _loadedFiles.Remove(obj.Layer.FileName);
        }

        private void RemoveSplitRoute(BikeTouringGISLayer obj)
        {
            obj.RemoveSplitRoutes();
        }

        private bool CanReSplitRoute(SplitLayerProperties parameters)
        {
            return parameters.CanReSplit;
        }

        private void SplitRoute(SplitLayerProperties obj)
        {
            if (obj.CanSplit)
            {
                obj.Layer.SplitRoute(obj.Distance);
            }
        }

        public bool ConvertTracksToRoutesAutomatically
        {
            get { return _convertTracksToRoutesAutomatically; }
            set { Set(ref _convertTracksToRoutesAutomatically, value); }
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
                    if (!_loadedFiles.Contains(file))
                    {
                        OpenGpxFile(mapViewModel, file);
                    }
                }
            }
        }

        internal async void OpenGpxFile(BikeTouringGISMapViewModel mapViewModel, string path)
        {
            if (_loadedFiles.Exists(x => x.Equals(path)))
            {
                return;
            }
            var gpxFileInformation = new GpxFileReader().LoadFile(path);
            foreach (var track in gpxFileInformation.Tracks)
            {
                bool convertTrack = ConvertTracksToRoutesAutomatically;
                var convertTrackDialogResult = convertTrack ? MessageDialogResult.Affirmative : MessageDialogResult.FirstAuxiliary;
                if (!convertTrack)
                {
                    StringBuilder textBuilder = new StringBuilder();
                    textBuilder.AppendLine($"Track {track.Name} is defined as track and not as route");
                    textBuilder.AppendLine();
                    textBuilder.AppendLine("routes are used by navigation-devices");
                    textBuilder.AppendLine("tracks are to register where you have been");
                    textBuilder.AppendLine();
                    textBuilder.AppendLine("Do you want to convert it to a route?");
                    convertTrackDialogResult = await ConvertTrackToRoute(textBuilder.ToString());
                }
                switch (convertTrackDialogResult)
                {
                    case MessageDialogResult.Affirmative:
                        track.IsConvertedToRoute = true;
                        break;
                    case MessageDialogResult.Negative:
                        break;
                    case MessageDialogResult.FirstAuxiliary:
                        return;
                }
            }
            _loadedFiles.Add(path);
            GeometryFactory.CreateGeometriesFromGpxFile(gpxFileInformation);
            var routes = LayerFactory.CreateRoutes(gpxFileInformation.Routes);
            mapViewModel.AddRoutes(routes);
            var tracks = LayerFactory.CreateTracks(gpxFileInformation.Tracks);
            mapViewModel.AddTracks(tracks);
            mapViewModel.AddPoIs(gpxFileInformation.WayPoints);
        }

        private async Task<MessageDialogResult> ConvertTrackToRoute(string text)
        {
            var window = Application.Current.MainWindow as MetroWindow;
            var dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Convert to route",
                NegativeButtonText = "Keep as track",
                FirstAuxiliaryButtonText = "Cancel"
            };
            return await window.ShowMessageAsync("Convert track to route", text, MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, dialogSettings);
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