using BikeTouringGIS.Controls;
using BikeTouringGIS.Messenges;
using BikeTouringGIS.ViewModels;
using Fluent;
using GalaSoft.MvvmLight.Messaging;
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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
            Messenger.Default.Register<VisibilityTabChangedMessage>(this,ChangeVisibilityTab);
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

        // Using a DependencyProperty as the backing store for SelectedLayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLayerProperty =
            DependencyProperty.Register("SelectedLayer", typeof(BikeTouringGISLayer), typeof(MainMenu), new PropertyMetadata(null));


        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(BikeTouringGISMapViewModel), typeof(MainMenu), new PropertyMetadata(null));

        private void ChangeVisibilityTab(VisibilityTabChangedMessage message)
        {
            var group = ribbonMenu.ContextualGroups.FirstOrDefault(x => x.Name.Equals(message.TabName));
            group.Visibility = group.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}