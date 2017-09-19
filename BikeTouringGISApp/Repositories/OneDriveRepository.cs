using BikeTouringGISApp.Library.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;
using WinUX.CloudServices.OneDrive;

namespace BikeTouringGISApp.Repositories
{
    internal class OneDriveRepository : OneDriveService
    {
        private const string LOGBOOKSDIRECTORYNAME = "LogBooks";
        private const string LOGSDIRECTORYNAME = "Logs";

        public OneDriveRepository() : base("theRightDirection/BikeTouringGIS", "00000000441DB51C", new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite" })
        {
        }

        internal async Task<IEnumerable<LogBook>> GetLogBooks()
        {
            return await GetItems<LogBook>(LOGBOOKSDIRECTORYNAME);
        }

        internal async Task<IEnumerable<Log>> GetLogs()
        {
            return await GetItems<Log>(LOGSDIRECTORYNAME);
        }

        internal async Task UpdateLog(Log log)
        {
        }

        internal async Task UpdateLogBook(LogBook logBook)
        {
        }

        private async Task<bool> ConnectToOneDrive()
        {
            var result = await Connect();
            if (result.IsConnected)
            {
                await CreateFolderAsync(LOGBOOKSDIRECTORYNAME);
                await CreateFolderAsync(LOGSDIRECTORYNAME);
            }
            return result.IsConnected;
        }

        private async Task<IEnumerable<T>> GetItems<T>(string folderName)
        {
            var items = new List<T>();
            try
            {
                var isConnected = await ConnectToOneDrive();
                if (isConnected)
                {
                    var content = await GetAllItemsAsync(LOGBOOKSDIRECTORYNAME);
                    content.ForEach(item =>
                    {
                        var realObject = JsonConvert.DeserializeObject<T>(item);
                        items.Add(realObject);
                    });
                }
            }
            catch (Exception e) { }
            return items;
        }
    }
}