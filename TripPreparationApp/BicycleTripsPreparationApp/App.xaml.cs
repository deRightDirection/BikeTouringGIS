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

namespace BicycleTripsPreparationApp
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Deployed applications must be licensed at the Basic level or greater (https://developers.arcgis.com/licensing).
                // To enable Basic level functionality set the Client ID property before initializing the ArcGIS Runtime.
                // ArcGISRuntimeEnvironment.ClientId = "<Your Client ID>";
                Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ClientId = "1KVc5mmIMY7okeFJ";

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

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it
            e.Handled = false;
#else
    ShowUnhandledException(e);    
#endif
        }

        private void ShowUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            string errorMessage = string.Format("An application error occurred.\nPlease check whether your data is correct and repeat the action. If this error occurs again there seems to be a more serious malfunction in the application, and you better close it.\n\nError:{0}\n\nDo you want to continue?\n(if you click Yes you will continue with your work, if you click No the application will close)",

            e.Exception.Message + (e.Exception.InnerException != null ? "\n" + e.Exception.InnerException.Message : null));

            ILogger logger = Logger.GetLogger();
            logger.LogException(e.Exception);

            if (MessageBox.Show(errorMessage, "Application Error", MessageBoxButton.YesNoCancel, MessageBoxImage.Error) == MessageBoxResult.No)
            {
                if (MessageBox.Show("WARNING: The application will close. ", "Close the application!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}