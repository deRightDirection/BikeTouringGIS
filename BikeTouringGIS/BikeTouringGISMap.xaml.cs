using BikeTouringGIS.Controls;
using BikeTouringGIS.ViewModels;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using theRightDirection.Library;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class BikeTouringGISMap : UserControl
    {
        private BikeTouringGISMapViewModel _vm;

        public BikeTouringGISMap()
        {
            InitializeComponent();
            Map = MapControl;
            var bindingViewMode = new Binding("Map") { Mode = BindingMode.TwoWay };
            SetBinding(MapProperty, bindingViewMode);
            bindingViewMode = new Binding("TotalLengthOfRoutes") { Mode = BindingMode.OneWay };
            SetBinding(TotalLengthOfRoutesProperty, bindingViewMode);
            bindingViewMode = new Binding("BikeTouringGISLayers") { Mode = BindingMode.OneWay };
            SetBinding(BikeTouringGISLayersProperty, bindingViewMode);
            _vm = ((BikeTouringGISMapViewModel)DataContext);
            _vm.MapView = MapViewControl;
            SetSymbology();
        }

        private void SetSymbology()
        {
            foreach (DictionaryEntry item in Resources)
            {
                GraphicType enumValue;
                EnumHelper.TryParseTextToEnumValue(item.Key.ToString(), out enumValue);
                if (enumValue != GraphicType.Unknown)
                {
                    _vm.AddSymbol(enumValue, item.Value);
                }
            }
        }

        public Map Map
        {
            get { return (Map)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(Map), typeof(BikeTouringGISMap), new PropertyMetadata(null, OnMapSet));

        private static void OnMapSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((BikeTouringGISMap)d).DataContext;
            ((BikeTouringGISMapViewModel)context).Map = e.NewValue as Map;
        }

        public MainMenu Menu
        {
            get { return (MainMenu)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MainMenu), typeof(BikeTouringGISMap), new PropertyMetadata(null, OnMenuSet));

        private static void OnMenuSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((BikeTouringGISMap)d).DataContext;
            ((BikeTouringGISMapViewModel)context).Menu = e.NewValue as MainMenu;
        }

        public ObservableCollection<BikeTouringGISLayer> BikeTouringGISLayers
        {
            get { return (ObservableCollection<BikeTouringGISLayer>)GetValue(BikeTouringGISLayersProperty); }
            set { SetValue(BikeTouringGISLayersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BikeTouringGISLayers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BikeTouringGISLayersProperty =
            DependencyProperty.Register("BikeTouringGISLayers", typeof(ObservableCollection<BikeTouringGISLayer>), typeof(BikeTouringGISMap), new PropertyMetadata(null));

        public int TotalLengthOfRoutes
        {
            get { return (int)GetValue(TotalLengthOfRoutesProperty); }
            set { SetValue(TotalLengthOfRoutesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalLengthOfRoutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalLengthOfRoutesProperty =
            DependencyProperty.Register("TotalLengthOfRoutes", typeof(int), typeof(BikeTouringGISMap), new PropertyMetadata(0));
    }
}