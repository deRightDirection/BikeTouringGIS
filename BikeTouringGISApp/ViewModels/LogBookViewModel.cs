using BikeTouringGISApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.ViewModels
{
    public class LogBookViewModel : BikeTouringGISBaseViewModel
    {
        private bool _isReadOnly;
        private LogBook _logBook;

        public LogBookViewModel()
        {
            DataFormIsInReadOnlyMode = true;
        }

        public bool DataFormIsInReadOnlyMode
        {
            get { return _isReadOnly; }
            set { Set(() => DataFormIsInReadOnlyMode, ref _isReadOnly, value); }
        }

        public LogBook LogBook
        {
            get { return _logBook; }
            set { Set(() => LogBook, ref _logBook, value); }
        }
    }
}