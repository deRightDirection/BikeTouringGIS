using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BikeTouringGIS.ViewModels
{
    public class LayerListViewModel : BikeTouringGISBaseViewModel
    {
        private ObservableCollection<BikeTouringGISLayer> _layers;
        private BikeTouringGISLayer _selectedLayer;

        public ObservableCollection<BikeTouringGISLayer> Layers
        {
            get { return _layers; }
            set
            {
                Set(ref _layers, value);
                if (_layers != null)
                {
                    _layers.CollectionChanged += _layers_CollectionChanged;
                }
                SetSelection();
            }
        }

        private void SetSelection()
        {
            if (_layers != null && SelectedLayer == null)
            {
                SelectedLayer = _layers.Where(x => x.Type == LayerType.GPXRoute).FirstOrDefault();
            }
        }

        private void _layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetSelection();
        }

        public BikeTouringGISLayer SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                if (_selectedLayer != value)
                {
                    _selectedLayer = value;
                    if (value?.Type == LayerType.PointsOfInterest)
                    {
                        _selectedLayer = null;
                    }
                    RaisePropertyChanged("SelectedLayer");
                }
            }
        }
    }
}