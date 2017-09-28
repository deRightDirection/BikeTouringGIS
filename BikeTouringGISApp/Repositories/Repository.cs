using BikeTouringGISApp.Library.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BikeTouringGISApp.Repositories
{
    public abstract class Repository<T> where T : IEntity<T>
    {
        private readonly string APPLICATIONFOLDERNAME;
        private readonly string FOLDERNAME;
        private StorageFolder MAINSYSTEMFOLDER;

        protected Repository(string folderName)
        {
            APPLICATIONFOLDERNAME = "BikeTouringGIS";
            FOLDERNAME = folderName;
            SetMainPath();
        }

        public string Path { get; private set; }

        public async Task Delete(T entity)
        {
            var folderToLoad = await CreateBaseFolder();
            var file = await folderToLoad.TryGetItemAsync($"{entity.Identifier.ToString()}.json");
            if (file != null)
            {
                await file.DeleteAsync();
            }
        }

        public async Task<IEnumerable<T>> GetData()
        {
            var folderToLoad = await CreateBaseFolder();
            var files = await folderToLoad.GetFilesAsync();
            var result = new List<T>();
            foreach (var file in files)
            {
                var text = await FileIO.ReadTextAsync(file);
                var item = JsonConvert.DeserializeObject<T>(text);
                result.Add(item);
            }
            return result;
        }

        public async Task Save(T entity)
        {
            var logAsJson = JsonConvert.SerializeObject(entity);
            var folderToSave = await CreateBaseFolder();
            var file = await folderToSave.CreateFileAsync($"{entity.Identifier.ToString()}.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, logAsJson);
        }

        private async Task<StorageFolder> CreateBaseFolder()
        {
            await SetMainPath();
            var baseFolder = await MAINSYSTEMFOLDER.CreateFolderAsync(APPLICATIONFOLDERNAME, CreationCollisionOption.OpenIfExists);
            var logsFolder = await baseFolder.CreateFolderAsync(FOLDERNAME, CreationCollisionOption.OpenIfExists);
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

        private async Task SetMainPath()
        {
            var mainSystemPath = await GetMainSystemPath();
            MAINSYSTEMFOLDER = mainSystemPath;
        }
    }
}