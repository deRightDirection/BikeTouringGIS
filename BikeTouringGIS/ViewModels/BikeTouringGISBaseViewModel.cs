using BikeTouringGIS.Messenges;
using Esri.ArcGISRuntime.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISBaseViewModel : ViewModelBase
    {
        private MainMenu _menu;
        protected Map _map;
        public RelayCommand<string> ChangeVisibilityTabGroupCommand { get; private set; }
        public BikeTouringGISBaseViewModel()
        {
            ChangeVisibilityTabGroupCommand = new RelayCommand<string>(ChangeVisibilityTabGroup);
        }

        private void ChangeVisibilityTabGroup(string tabGroupName)
        {
            MessengerInstance.Send(new VisibilityTabChangedMessage() { TabName = tabGroupName });
        }

        public MainMenu Menu
        {
            set { Set(ref _menu, value); }
        }

        public Map Map
        {
            get { return _map; }
            set { Set(ref _map, value); }
        }
    }
}