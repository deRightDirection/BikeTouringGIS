using BikeTouringGIS.Controls;
using BikeTouringGIS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BikeTouringGISViewModel Base
        {
            get { return (BikeTouringGISViewModel)GetValue(BaseProperty); }
            set { SetValue(BaseProperty, value); }
        }

        public BikeTouringGISMapViewModel Map
        {
            get { return (BikeTouringGISMapViewModel)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        public BikeTouringGISLayer SelectedLayer
        {
            get { return (BikeTouringGISLayer)GetValue(SelectedLayerProperty); }
            set { SetValue(SelectedLayerProperty, value); }
        }

        public static readonly DependencyProperty SelectedLayerProperty =
            DependencyProperty.Register("SelectedLayer", typeof(BikeTouringGISLayer), typeof(MainMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(BikeTouringGISMapViewModel), typeof(MainMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty BaseProperty =
        DependencyProperty.Register("Base", typeof(BikeTouringGISViewModel), typeof(MainMenu), new PropertyMetadata(null));
    }
}