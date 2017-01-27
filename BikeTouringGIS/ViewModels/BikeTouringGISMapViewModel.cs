using BikeTouringGIS.Controls;
using BikeTouringGIS.Messenges;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.ViewModels
{
    public class BikeTouringGISMapViewModel : BikeTouringGISBaseViewModel
    {
        public BikeTouringGISMapViewModel()
        {
            MessengerInstance.Register<GPXDataLoadedMessage>(this, LoadGPXData);
        }

        private void LoadGPXData(GPXDataLoadedMessage message)
        {
            _map.Layers.Add(message.Layer);
        }
    }
}