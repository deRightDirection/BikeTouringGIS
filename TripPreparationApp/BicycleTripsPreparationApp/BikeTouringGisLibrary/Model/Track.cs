using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;

namespace BikeTouringGISLibrary.Model
{
    public class Track : Route
    {
        public trksegType[] Segments { get; internal set; }
        public bool IsConvertedToRoute { get; private set; }

        public void ConvertTrackToRoute()
        {
            var waypoints = new List<wptType>();
            foreach (var segment in Segments)
            {
                waypoints.AddRange(segment.trkpt);
            }
            Points = waypoints;
            IsConvertedToRoute = true;
        }
    }
}