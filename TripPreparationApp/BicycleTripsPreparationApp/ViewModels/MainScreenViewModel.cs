using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.ViewModels
{
    public class MainScreenViewModel : ViewModelBase
    {
        private bool _showOpenCycleMap, _showOpenStreetMap, _showFietsknooppunten;
        public MainScreenViewModel()
        {
            SwitchBaseMapCommand = new RelayCommand<int>(x => SwitchBaseMap(x));
        }

        public bool ShowOpenCycleMap
        {
            get { return _showOpenCycleMap; }
            set { Set(() => ShowOpenCycleMap, ref _showOpenCycleMap, value); }
        }
        public bool ShowOpenStreetMap
        {
            get { return _showOpenStreetMap; }
            set { Set(() => ShowOpenStreetMap, ref _showOpenStreetMap, value); }
        }
        public bool ShowFietsknooppunten
        {
            get { return _showFietsknooppunten; }
            set { Set(() => ShowFietsknooppunten, ref _showFietsknooppunten, value); }
        }

        private void SwitchBaseMap(int baseMapParameter)
        {
            switch(baseMapParameter)
            {
                case 0: ShowOpenCycleMap = true;
                    ShowOpenStreetMap = false;
                    ShowFietsknooppunten = false;
                    break;
                case 1:ShowOpenCycleMap = false;
                    ShowOpenStreetMap = true;
                    ShowFietsknooppunten = false;
                    break;
                case 2: ShowOpenCycleMap = false;
                    ShowOpenStreetMap = false;
                    ShowFietsknooppunten = true;
                    break;
            }
        }

        public RelayCommand<int> SwitchBaseMapCommand { get; private set; }
    }
}