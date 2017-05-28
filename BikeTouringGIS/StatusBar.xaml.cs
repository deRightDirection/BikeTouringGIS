using BikeTouringGIS.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        public BikeTouringGISMapViewModel Map
        {
            get { return (BikeTouringGISMapViewModel)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(BikeTouringGISMapViewModel), typeof(StatusBar), new PropertyMetadata(null));
    }
}