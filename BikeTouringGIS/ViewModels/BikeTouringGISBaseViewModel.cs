using Esri.ArcGISRuntime.Controls;
using GalaSoft.MvvmLight;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISBaseViewModel : ViewModelBase
    {
        private MainMenu _menu;
        protected Map _map;

        public BikeTouringGISBaseViewModel()
        {
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