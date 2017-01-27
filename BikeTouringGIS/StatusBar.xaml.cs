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
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        public MapView MapView
        {
            get { return (MapView)GetValue(MapViewProperty); }
            set { SetValue(MapViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapViewProperty =
            DependencyProperty.Register("MapView", typeof(MapView), typeof(StatusBar), new PropertyMetadata(null, OnMapViewSet));

        private static void OnMapViewSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((StatusBar)d).DataContext;
            ((StatusBarViewModel)context).MapView = e.NewValue as MapView;
        }
    }
}
