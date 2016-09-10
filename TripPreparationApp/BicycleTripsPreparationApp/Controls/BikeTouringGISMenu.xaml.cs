using BikeTouringGIS.ViewModels;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class BikeTouringGISMenu : UserControl
    {
        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(BikeTouringGISProject), typeof(BikeTouringGISMenu), new PropertyMetadata(default(BikeTouringGISProject)));
        private readonly IDialogCoordinator _dialogCoordinator;

        public BikeTouringGISMenu()
        {
            InitializeComponent();
            _dialogCoordinator = DialogCoordinator.Instance;
        }

        public BikeTouringGISProject Project
        {
            get { return (BikeTouringGISProject)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveProjectFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveProjectFileDialog.Title = "Save a new Bike touring GIS project";
            saveProjectFileDialog.Filter = "Bike touring GIS project files (*.biketouringgis)|*.biketouringgis";
            saveProjectFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (saveProjectFileDialog.ShowDialog() == true)
            {
                var newProjectDialog = new CustomDialog() { Title = "Create new Bike touring GIS project" };
                var mainWindow = FindMainScreen() as MainScreen;
                var newProjectDialogContent = new NewProjectDialogContent(saveProjectFileDialog.FileName, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(mainWindow.DataContext, newProjectDialog);
                    Project = instance;
                    BikeTouringGISProjectFileHandler.SaveNewProjectFile(instance);
                },() =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(mainWindow.DataContext, newProjectDialog);
                });
                newProjectDialog.Content = new NewProjectDialog { DataContext = newProjectDialogContent};
                await _dialogCoordinator.ShowMetroDialogAsync(mainWindow.DataContext, newProjectDialog);
            }
        }

        private DependencyObject FindMainScreen()
        {
            DependencyObject ucParent = Parent;
            while (!(ucParent is MainScreen))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }
            return ucParent;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openProjectFileDialog = new Microsoft.Win32.OpenFileDialog();
            openProjectFileDialog.Title = "Open a Bike touring GIS project";
            openProjectFileDialog.Filter = "Bike touring GIS project files (*.biketouringgis)|*.biketouringgis";
            openProjectFileDialog.InitialDirectory = DropBoxHelper.GetDropBoxFolder();
            if (openProjectFileDialog.ShowDialog() == true)
            {
                var fileName = openProjectFileDialog.FileName;
                Project = BikeTouringGISProjectFileHandler.OpenProjectFile(fileName);
            }
        }
    }
}
