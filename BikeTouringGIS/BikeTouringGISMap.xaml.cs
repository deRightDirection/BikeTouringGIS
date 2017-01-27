using BikeTouringGIS.ViewModels;
using Esri.ArcGISRuntime.Controls;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class BikeTouringGISMap : UserControl
    {
        public BikeTouringGISMap()
        {
            InitializeComponent();
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

        public Map Map
        {
            get { return MapControl; }
        }

        public MapView MapView
        {
            get { return MapViewControl; }
        }
    }
}
