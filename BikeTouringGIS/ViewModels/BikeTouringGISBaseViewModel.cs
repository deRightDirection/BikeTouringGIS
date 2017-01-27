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
        public RelayCommand<string> ShowTabGroupCommand { get; private set; }
        public RelayCommand<string> CloseTabGroupCommand { get; private set; }
        public BikeTouringGISBaseViewModel()
        {
            ShowTabGroupCommand = new RelayCommand<string>(ShowTabGroup);
            CloseTabGroupCommand = new RelayCommand<string>(CloseTabGroup);
        }
        private void ShowTabGroup(string tabGroupName)
        {
            _menu.ShowTabGroup(tabGroupName);
        }

        private void CloseTabGroup(string tabGroupName)
        {
            _menu.HideTabGroup(tabGroupName);
        }

        public MainMenu Menu
        {
            set { Set(ref _menu, value); }
        }

        public Map Map
        {
            set { Set(ref _map, value); }
        }
    }
}