﻿using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.GPX;
using System.Collections.Generic;
using theRightDirection.Library;

namespace BikeTouringGISLibrary.Model
{
    public class Track : GeometryData, IPath
    {
        private bool _isConverted;
        public Track()
        {
            Type = PathType.Track;
        }
        public BikeTouringGISGraphic StartLocation { get; internal set; }
        public BikeTouringGISGraphic EndLocation { get; internal set; }
        public bool IsConvertedToRoute
        {
            get { return _isConverted; }
            set
            {
                if (value != _isConverted)
                {
                    _isConverted = value;
                    Type = _isConverted ? PathType.Route : PathType.Track;
                }
            }
        }
        public trksegType[] Segments
        {
            set
            {
                var waypoints = new List<wptType>();
                foreach (var segment in value)
                {
                    waypoints.AddRange(segment.trkpt);
                }
                Points = waypoints;
            }
        }
        public PathType Type { get; set; }
        public Route ConvertTrack()
        {
            var newRoute = new Route();
            this.CopyProperties(newRoute, true, new string[] { "Segments" });
            newRoute.Type = PathType.Route;
            return newRoute;
        }
    }
}