using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using theRightDirection.Library.Logging;

namespace BikeTouringGIS
{
        public static class DropBoxHelper
        {
            private static string _folder;
            public static string GetDropBoxFolder()
            {
                if (string.IsNullOrEmpty(_folder))
                {
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dropbox\\host.db");
                    if (VersionHelpers.IsWindows10OrGreater())
                    {
                        dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dropbox\\host.db");
                    }
                    try
                    {
                        var dbBase64Text = Convert.FromBase64String(File.ReadAllText(dbPath));
                        var lastIndex = 0;
                        for (int i = 0; i < dbBase64Text.Length; i++)
                        {
                            if (dbBase64Text[i] == 52)
                            {
                                lastIndex = i;
                            }
                        }
                        var path = dbBase64Text.Skip(lastIndex + 1).ToArray();
                        _folder = Encoding.UTF8.GetString(path);
                    }
                    catch (Exception e)
                    {
                        ILogger logger = Logger.GetLogger();
                        logger.LogException(e);
                        return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    }
                }
                return _folder;
            }
    }
}
