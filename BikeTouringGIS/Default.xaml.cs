﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Squirrel;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using theRightDirection.Library.Logging;
using Esri.ArcGISRuntime.Layers;
using BikeTouringGIS.ViewModels;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for Default.xaml
    /// </summary>
    public partial class Default : MetroWindow
    {
        public Default()
        {
            InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/MannusEtten/BikeTouringGIS"))
                {
                    var updateInfo = await mgr.CheckForUpdate();
                    var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;
                    var futureVersion = updateInfo.FutureReleaseEntry.Version;
                    if (currentVersion != futureVersion)
                    {
                        var window = Application.Current.MainWindow as MetroWindow;
                        var controller = await window.ShowProgressAsync("Please wait...", "Updating application");
                        await mgr.UpdateApp();
                        await controller.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ILogger logger = Logger.GetLogger();
                logger.LogException(ex);
            }
        }
    }
}