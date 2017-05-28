using BikeTouringGISLibrary.Model;
using Newtonsoft.Json;
using System.IO;

namespace BikeTouringGISLibrary
{
    public static class BikeTouringGISProjectFileHandler
    {
        public static BikeTouringGISProject OpenProjectFile(string fileName)
        {
            var data = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<BikeTouringGISProject>(data);
        }

        public static void SaveProjectFile(BikeTouringGISProject project)
        {
            var data = JsonConvert.SerializeObject(project);
            File.WriteAllText(project.ProjectFileLocation, data);
        }
    }
}