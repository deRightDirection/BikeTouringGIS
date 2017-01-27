using BikeTouringGISLibrary;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISViewModel : BikeTouringGISBaseViewModel
    {
        public RelayCommand OpenGPXFileCommand { get; private set; }
        public BikeTouringGISViewModel()
        {
            OpenGPXFileCommand = new RelayCommand(OpenGPXFile);
        }

        private async void OpenGPXFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openFileDialog.ShowDialog() == true)
            {
                var gpxFileInformation = new GpxFileReader().LoadFile(openFileDialog.FileName);
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
    }
}