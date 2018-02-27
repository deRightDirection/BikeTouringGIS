using BikeTouringGISApp.Library.Enumerations;
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
            var items = await GetItems<LogBook>(LOGBOOKSDIRECTORYNAME);
            items.ForEach(x => x.Source = RepositorySource.OneDrive);
            return items;
        }

        internal async Task<IEnumerable<Log>> GetLogs()
        {
            return await GetItems<Log>(LOGSDIRECTORYNAME);
        }

        internal async Task SaveOrUpdateLog(Log log)
        {
            var isConnected = await ConnectToOneDrive();
            if (isConnected)
            {
                await UpdateAsync($"{log.Identifier}.json", JsonConvert.SerializeObject(log), LOGSDIRECTORYNAME);
            }
        }

        internal async Task SaveOrUpdateLogBook(LogBook logBook)
        {
            var isConnected = await ConnectToOneDrive();
            if (isConnected)
            {
                await UpdateAsync($"{logBook.Identifier}.json", JsonConvert.SerializeObject(logBook), LOGBOOKSDIRECTORYNAME);
            }
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
                    var content = await GetAllItemsAsync(folderName);
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