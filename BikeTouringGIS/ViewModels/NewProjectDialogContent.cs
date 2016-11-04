using BikeTouringGISLibrary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace BikeTouringGIS.ViewModels
{
    public class NewProjectDialogContent : ViewModelBase
    {

        private string _projectName;
        private string _description;
        private Action<BikeTouringGISProject> _closeAction;
        private string _projectFileLocation;

        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public NewProjectDialogContent(string projectFileLocation, Action<BikeTouringGISProject> closeAndOkHandler, Action closeAndCancelHandler)
        {
            _projectFileLocation = projectFileLocation;
            _closeAction = closeAndOkHandler;
            CloseCommand = new RelayCommand(DoIets, CanDoIets);
            CancelCommand = new RelayCommand(closeAndCancelHandler);
            ProjectName = string.Empty;
        }

        private bool CanDoIets()
        {
            return ProjectName.Trim().Length >= 5;
        }

        private void DoIets()
        {
            _closeAction.Invoke(new BikeTouringGISProject() { Description = Description, Name = ProjectName, ProjectFileLocation = _projectFileLocation });
        }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                Set(() => ProjectName, ref _projectName, value);
                CloseCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }

//        public event PropertyChangedEventHandler PropertyChanged;

 //       [NotifyPropertyChangedInvocator]
  //      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   //     {
    //        var handler = PropertyChanged;
     //       if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
      //  }
    }
}
