using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX.CloudServices;
using WinUX.CloudServices.OneDrive;

namespace BikeTouringGISApp.ViewModels
{
    public abstract class BikeTouringGISBaseViewModel : ViewModelBase
    {
        protected ICloudService _oneDrive;
        private bool _oneDriveIsOnline;

        public BikeTouringGISBaseViewModel()
        {
            _oneDrive = SimpleIoc.Default.GetInstance<ICloudService>();
            CheckOneDrive();
        }

        public bool OneDriveIsOnline
        {
            get { return _oneDriveIsOnline; }
            set
            {
                Set(ref _oneDriveIsOnline, value);
            }
        }

        private async void CheckOneDrive()
        {
            var resultOneDrive = await _oneDrive.Connect();
            OneDriveIsOnline = resultOneDrive.IsConnected;
        }
    }
}