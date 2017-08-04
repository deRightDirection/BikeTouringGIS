using GalaSoft.MvvmLight.Ioc;
using System;
using Template10.Common;
using Template10.Services.SettingsService;
using Template10.Utils;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace BikeTouringGISApp.Services
{
    public class SettingsService
    {
        private ISettingsHelper _helper;

        private SettingsService()
        {
            _helper = new SettingsHelper();
        }

        public static SettingsService Instance { get; } = new SettingsService();

        public GeolocationAccessStatus AllowUseGPSDevice
        {
            get { return _helper.Read("AllowUseGPSDevice", GeolocationAccessStatus.Unspecified, SettingsStrategies.Roam); }
            set { _helper.Write("AllowUseGPSDevice", value, SettingsStrategies.Roam); }
        }
    }
}