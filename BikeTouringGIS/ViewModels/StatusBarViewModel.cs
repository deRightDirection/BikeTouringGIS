using BikeTouringGIS.Messenges;
using BikeTouringGISLibrary.Enumerations;

namespace BikeTouringGIS.ViewModels
{
    public class StatusBarViewModel : BikeTouringGISBaseViewModel
    {
        private int _totalLengthOfRoutes, _selectedRouteLength;
        private string _versionInformation;
        private double _zoom;

        public StatusBarViewModel()
        {
            _zoom = 1;
        }

        public int SelectedRouteLength
        {
            get { return _selectedRouteLength; }
            set { Set(ref _selectedRouteLength, value); }
        }

        public int TotalLengthOfRoutes
        {
            get { return _totalLengthOfRoutes; }
            set { Set(ref _totalLengthOfRoutes, value); }
        }

        public string VersionInformation
        {
            get { return _versionInformation; }
            set { Set(ref _versionInformation, value); }
        }

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                ZoomMap(1 - (value - _zoom));
                Set(ref _zoom, value);
            }
        }

        private void ZoomMap(double zoomFactor)
        {
            var message = new ExtentChangedMessage(ExtentChangedReason.StatusBarZoomInOrZoomOut) { ZoomFactor = zoomFactor };
            MessengerInstance.Send(message);
        }
    }
}