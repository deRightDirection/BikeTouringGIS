using BikeTouringGISApp.Library.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BikeTouringGISApp.Services
{
    internal class FileService
    {
        private readonly string APPLICATIONFOLDERNAME;
        private readonly string LOGSFOLDERNAME;
        private StorageFolder MAINSYSTEMFOLDER;

        private FileService()
        {
            APPLICATIONFOLDERNAME = "BikeTouringGIS";
            LOGSFOLDERNAME = "Logs";
        }

        public static FileService Instance { get; } = new FileService();
        public string Path { get; private set; }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            var folderToLoad = await CreateBaseFolder();
            var files = await folderToLoad.GetFilesAsync();
            var result = new List<Log>();
            foreach (var file in files)
            {
                var text = await FileIO.ReadTextAsync(file);
                var log = JsonConvert.DeserializeObject<Log>(text);
                result.Add(log);
            }
            return result;
        }

        public async Task SaveLog(Log log)
        {
            var logAsJson = JsonConvert.SerializeObject(log);
            var folderToSave = await CreateBaseFolder();
            var file = await folderToSave.CreateFileAsync(log.FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, logAsJson);
        }

        public async Task SetMainFolder()
        {
            var mainSystemPath = await GetMainSystemPath();
            MAINSYSTEMFOLDER = mainSystemPath;
        }

        private async Task<StorageFolder> CreateBaseFolder()
        {
            var baseFolder = await MAINSYSTEMFOLDER.CreateFolderAsync(APPLICATIONFOLDERNAME, CreationCollisionOption.OpenIfExists);
            var logsFolder = await baseFolder.CreateFolderAsync(LOGSFOLDERNAME, CreationCollisionOption.OpenIfExists);
            Path = logsFolder.Path;
            return logsFolder;
        }

        private async Task<StorageFolder> GetMainSystemPath()
        {
            var removableDevices = KnownFolders.RemovableDevices;
            var removableFolders = await removableDevices.GetFoldersAsync();
            var sdCard = removableFolders.FirstOrDefault();
            if (sdCard != null && sdCard.Attributes.HasFlag(FileAttributes.ReadOnly) == false)
            {
                var foldersOnSdCard = await sdCard.GetFoldersAsync();
                var documents = foldersOnSdCard.Where(x => x.DisplayName.Equals("Documents")).First();
                return documents;
            }
            return ApplicationData.Current.RoamingFolder;
        }
    }
}