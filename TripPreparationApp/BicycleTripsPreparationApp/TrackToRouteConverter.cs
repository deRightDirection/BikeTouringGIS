using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPX;

namespace BikeTouringGIS
{
    class TrackToRouteConverter
    {
       internal List<wptType> ConvertTrackToRoute(trkType track)
        {
            var waypoints = new List<wptType>();
            foreach(var segment in track.trkseg)
            {
                waypoints.AddRange(segment.trkpt);
            }
            return waypoints;
        }
    }
}
