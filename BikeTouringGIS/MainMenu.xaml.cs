using BikeTouringGIS.Messenges;
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
            Messenger.Default.Register<VisibilityTabChangedMessage>(this,ChangeVisibilityTab);
        }

        private void ChangeVisibilityTab(VisibilityTabChangedMessage message)
        {
            var group = ribbonMenu.ContextualGroups.FirstOrDefault(x => x.Name.Equals(message.TabName));
            group.Visibility = group.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}