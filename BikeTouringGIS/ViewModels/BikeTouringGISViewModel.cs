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