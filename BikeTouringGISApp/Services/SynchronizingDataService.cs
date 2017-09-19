using BikeTouringGISApp.Library.Model;
using BikeTouringGISApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Services
{
    internal class SynchronizingDataService
    {
        private OneDriveRepository _oneDriveRepository;

        public SynchronizingDataService()
        {
            _oneDriveRepository = new OneDriveRepository();
        }

        public void SaveToOneDrive(LogBook logbook)
        {
        }

        public void SaveToOneDrive(Log log)
        {
        }

        public void SynchronizeLogBooks()
        {
        }

        public void SynchronizeLogs()
        {
        }
    }
}