using BicycleTripsPreparationApp;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BikeTouringGIS.ViewModels
{
    public class MainScreenViewModel : ViewModelBase
    {



        /*
        // TODO: nog niet volledig mvvm
        private async void AddPOI()
        {

            await _dialogCoordinator.ShowInputAsync(this, "Name of POI", "Fill in the name of POI and click ok.")
                .ContinueWith(t => _poiName = t.Result);
            if(string.IsNullOrEmpty(_poiName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Name of POI", "No name for POI given");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Name of POI", "After closing this window double click on the map for the location of the POI");
                _poiClickMode = true;

            }
        }
        */

    }
}