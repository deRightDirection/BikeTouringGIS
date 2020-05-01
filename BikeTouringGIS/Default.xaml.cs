using BikeTouringGIS.ViewModels;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for Default.xaml
    /// </summary>
    public partial class Default : MetroWindow
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Default()
        {
            InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var context = (BikeTouringGISViewModel)DataContext;
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("settings", FileMode.OpenOrCreate, isolatedStorage));
                if (srReader != null)
                {
                    string data = srReader.ReadToEnd();
                    if (string.IsNullOrEmpty(data))
                    {
                        context.SplitLength = 100;
                        context.ConvertTracksToRoutesAutomatically = false;
                    }
                    else
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(data);
                        context.SplitLength = jsonObject.SplitLength;
                        context.ConvertTracksToRoutesAutomatically = jsonObject.ConvertTracksToRoutesAutomatically;
                    }
                    srReader.Close();
                }
            }
            catch (Exception ex)
            {
                context.SplitLength = 100;
                _log.Error(ex);
            }
        }
    }
}