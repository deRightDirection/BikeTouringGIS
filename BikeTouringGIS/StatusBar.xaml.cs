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
            var bindingViewMode = new Binding("TotalLengthOfRoutes") { Mode = BindingMode.OneWay };
            SetBinding(TotalLengthOfRoutesProperty, bindingViewMode);
            bindingViewMode = new Binding("VersionInformation") { Mode = BindingMode.OneWay };
            SetBinding(VersionInformationProperty, bindingViewMode);
            bindingViewMode = new Binding("SelectedRouteLength") { Mode = BindingMode.OneWay };
            SetBinding(SelectedRouteLengthProperty, bindingViewMode);
        }

        public MapView MapView
        {
            get { return (MapView)GetValue(MapViewProperty); }
            set { SetValue(MapViewProperty, value); }
        }

        public int TotalLengthOfRoutes
        {
            get { return (int)GetValue(TotalLengthOfRoutesProperty); }
            set { SetValue(TotalLengthOfRoutesProperty, value); }
        }

        public int SelectedRouteLength
        {
            get { return (int)GetValue(SelectedRouteLengthProperty); }
            set { SetValue(SelectedRouteLengthProperty, value); }
        }

        public string VersionInformation
        {
            get { return (string)GetValue(VersionInformationProperty); }
            set { SetValue(VersionInformationProperty, value); }
        }

        public static readonly DependencyProperty SelectedRouteLengthProperty =
            DependencyProperty.Register("SelectedRouteLength", typeof(int), typeof(StatusBar), new PropertyMetadata(0, OnSelectedRouteSet));

        public static readonly DependencyProperty VersionInformationProperty =
            DependencyProperty.Register("VersionInformation", typeof(string), typeof(StatusBar), new PropertyMetadata(string.Empty, OnVersionInformationSet));

        private static void OnVersionInformationSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((StatusBar)d).DataContext;
            ((StatusBarViewModel)context).VersionInformation = (string)e.NewValue;
        }

        // Using a DependencyProperty as the backing store for TotalLengthOfRoutes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalLengthOfRoutesProperty =
            DependencyProperty.Register("TotalLengthOfRoutes", typeof(int), typeof(StatusBar), new PropertyMetadata(0, OnLengthSet));

        private static void OnLengthSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((StatusBar)d).DataContext;
            ((StatusBarViewModel)context).TotalLengthOfRoutes = (int)e.NewValue;
        }

        private static void OnSelectedRouteSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((StatusBar)d).DataContext;
            ((StatusBarViewModel)context).SelectedRouteLength = (int)e.NewValue;
        }

        public static readonly DependencyProperty MapViewProperty =
            DependencyProperty.Register("MapView", typeof(MapView), typeof(StatusBar), new PropertyMetadata(null, OnMapViewSet));

        private static void OnMapViewSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = ((StatusBar)d).DataContext;
            ((StatusBarViewModel)context).MapView = e.NewValue as MapView;
        }
    }
}
