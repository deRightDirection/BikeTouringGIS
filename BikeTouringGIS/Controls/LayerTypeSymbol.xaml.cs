using BikeTouringGISLibrary.Enumerations;
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

namespace BikeTouringGIS.Controls
{
    /// <summary>
    /// Interaction logic for LayerTypeSymbol.xaml
    /// </summary>
    public partial class LayerTypeSymbol : UserControl
    {
        public LayerTypeSymbol()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public LayerType Type
        {
            get { return (LayerType)GetValue(LayerTypeProperty); }
            set { SetValue(LayerTypeProperty, value); }
        }

        public static readonly DependencyProperty LayerTypeProperty =
            DependencyProperty.Register("Type", typeof(LayerType), typeof(LayerTypeSymbol), new PropertyMetadata(LayerType.Unknown));
    }
}
