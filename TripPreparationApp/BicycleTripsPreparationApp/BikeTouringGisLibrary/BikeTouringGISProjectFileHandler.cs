using BikeTouringGISLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISLibrary
{
    public static class BikeTouringGISProjectFileHandler
    {
        public static void SaveNewProjectFile(BikeTouringGISProject project)
        {
            var data = JsonConvert.SerializeObject(project);
            File.WriteAllText(project.ProjectFileLocation, data);
        }

        public static BikeTouringGISProject OpenProjectFile(string fileName)
        {
            var data = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<BikeTouringGISProject>(data);
        }
    }
}