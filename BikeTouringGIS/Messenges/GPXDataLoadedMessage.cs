using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Model;

namespace BikeTouringGIS.Messenges
{
    class GPXDataLoadedMessage
    {
        public BikeTouringGISLayer Layer { get; internal set; }
    }
}
