using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Services
{
    internal class NetworkService
    {
        internal static async Task<bool> IsInternetConnectionAvailable()
        {
            return NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
        }
    }
}