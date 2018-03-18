using BikeTouringGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGIS.Messenges
{
    class TrackDurationChangedMessage
    {
        public TrackDurationChangedMessage(TimeSpan value, Controls.BikeTouringGISLayer bikeTouringGISLayer)
        {
            Duration = value;
            Layer = bikeTouringGISLayer;
        }
        public TimeSpan Duration { get; private set; }
        public BikeTouringGISLayer Layer { get; private set; }
    }
}