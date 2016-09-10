using BikeTouringGISLibrary.Model;
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
    /// Interaction logic for ProjectDetails.xaml
    /// </summary>
    public partial class ProjectDetails : UserControl
    {
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(BikeTouringGISProject), typeof(ProjectDetails), new PropertyMetadata(default(BikeTouringGISProject)));
        public BikeTouringGISProject Project
        {
            get { return (BikeTouringGISProject)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public ProjectDetails()
        {
            InitializeComponent();
        }
    }
}
