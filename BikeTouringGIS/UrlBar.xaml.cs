using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for UrlBar.xaml
    /// </summary>
    public partial class UrlBar : UserControl
    {
        public UrlBar()
        {
            InitializeComponent();
        }

        private void OpenWebsite(object sender, RequestNavigateEventArgs e)
        {
            var hyperlink = (Hyperlink)e.Source;
            Process.Start(hyperlink.NavigateUri.AbsoluteUri);
        }
    }
}