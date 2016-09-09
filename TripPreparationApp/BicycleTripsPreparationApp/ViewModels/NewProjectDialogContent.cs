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
        private Action<NewProjectDialogContent> _closeAction;

        public RelayCommand CloseCommand { get; private set; }

        public NewProjectDialogContent(Action<NewProjectDialogContent> closeHandler)
        {
            _closeAction = closeHandler;
            CloseCommand = new RelayCommand(DoIets);
        }

        private void DoIets()
        {
            _closeAction.Invoke(this);
        }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
