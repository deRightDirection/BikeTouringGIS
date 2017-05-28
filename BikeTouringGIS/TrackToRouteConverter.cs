using GPX;
using System.Collections.Generic;

namespace BikeTouringGIS
{
    internal class TrackToRouteConverter
    {
        internal List<wptType> ConvertTrackToRoute(trkType track)
        {
            var waypoints = new List<wptType>();
            foreach (var segment in track.trkseg)
            {
                waypoints.AddRange(segment.trkpt);
            }
            return waypoints;
        }
    }
}