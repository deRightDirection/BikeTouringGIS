using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.Model
{
    public class Track : Route
    {
        public bool IsConvertedToRoute { get; private set; }
        public trksegType[] Segments { get; internal set; }

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