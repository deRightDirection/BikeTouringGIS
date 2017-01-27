using Esri.ArcGISRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using theRightDirection.Library.Logging;
using System.Windows.Threading;
using System.Windows.Media;

namespace BicycleTripsPreparationApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ex) => LogUnhandledException((Exception)ex.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            DispatcherUnhandledException += (s, ex) => LogUnhandledException(ex.Exception, "Application.Current.DispatcherUnhandledException");
            try
            {
                var x = Application.Current.Resources["RibbonThemeColorBrush"] = new SolidColorBrush(Colors.DarkGreen);

                // Deployed applications must be licensed at the Basic level or greater (https://developers.arcgis.com/licensing).
                // To enable Basic level functionality set the Client ID property before initializing the ArcGIS Runtime.
                // ArcGISRuntimeEnvironment.ClientId = "<Your Client ID>";
                ArcGISRuntimeEnvironment.ClientId = "1KVc5mmIMY7okeFJ";

                // Initialize the ArcGIS Runtime before any components are created.
                ArcGISRuntimeEnvironment.Initialize();

                // Standard level functionality can be enabled once the ArcGIS Runtime is initialized.                
                // To enable Standard level functionality you must either:
                // 1. Allow the app user to authenticate with ArcGIS Online or Portal for ArcGIS then call the set license method with their license info.
                // ArcGISRuntimeEnvironment.License.SetLicense(LicenseInfo object returned from an ArcGIS Portal instance)
                // 2. Call the set license method with a license string obtained from Esri Customer Service or your local Esri distributor.
                // ArcGISRuntimeEnvironment.License.SetLicense("<Your License String or Strings (extensions) Here>");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ArcGIS Runtime initialization failed.");
                ILogger logger = Logger.GetLogger();
                logger.LogException(ex);

                // Exit application
                this.Shutdown();
            }
        }

        private void LogUnhandledException(Exception exception, string @event)
        {
#if DEBUG
#else
            ILogger logger = Logger.GetLogger();
            logger.LogException(exception);
            this.Shutdown();
#endif
        }
    }
}