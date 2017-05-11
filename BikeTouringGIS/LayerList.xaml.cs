using BikeTouringGIS.Controls;
using BikeTouringGIS.ViewModels;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for LayerList.xaml
    /// </summary>
    public partial class LayerList : UserControl
    {
        public LayerList()
        {
            InitializeComponent();
            var bindingViewMode = new Binding("SelectedLayer") { Mode = BindingMode.TwoWay };
            SetBinding(SelectedLayerProperty, bindingViewMode);
        }

        public ObservableCollection<BikeTouringGISLayer> Layers
        {
            get { return (ObservableCollection<BikeTouringGISLayer>)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public BikeTouringGISLayer SelectedLayer
        {
            get { return (BikeTouringGISLayer)GetValue(SelectedLayerProperty); }
            set { SetValue(SelectedLayerProperty, value); }
        }

        public MainMenu Menu
        {
            get { return (MainMenu)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        public BikeTouringGISMapViewModel Map
        {
            get { return (BikeTouringGISMapViewModel)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(BikeTouringGISMapViewModel), typeof(LayerList), new PropertyMetadata(null));

        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MainMenu), typeof(LayerList), new PropertyMetadata(null, OnMenuSet));

        private static void OnMenuSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((LayerList)d).DataContext;
            ((LayerListViewModel)context).Menu = e.NewValue as MainMenu;
        }

        public static readonly DependencyProperty SelectedLayerProperty =
            DependencyProperty.Register("SelectedLayer", typeof(BikeTouringGISLayer), typeof(LayerList), new PropertyMetadata(null, OnLayerSet));

        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register("Layers", typeof(ObservableCollection<BikeTouringGISLayer>), typeof(LayerList), new PropertyMetadata(null, OnLayersSet));

        private static void OnLayerSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((LayerList)d).DataContext;
            ((LayerListViewModel)context).SelectedLayer= e.NewValue as BikeTouringGISLayer;
        }

        private static void OnLayersSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((LayerList)d).DataContext;
            ((LayerListViewModel)context).Layers = e.NewValue as ObservableCollection<BikeTouringGISLayer>;
        }
    }
}
