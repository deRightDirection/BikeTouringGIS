using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private bool _oneDriveIsOnline;

        public ShellViewModel()
        {
            OneDriveIsOnline = true;
        }

        public bool OneDriveIsOnline
        {
            get { return _oneDriveIsOnline; }
            set
            {
                Set(ref _oneDriveIsOnline, value);
            }
        }
    }
}