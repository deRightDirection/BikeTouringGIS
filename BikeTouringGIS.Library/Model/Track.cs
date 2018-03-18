using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System;
using System.Collections.Generic;
using WinUX;
namespace BikeTouringGISLibrary.Model
{
    public class Track : GeometryData, IPath
    {
        private bool _isConverted;
        private trksegType[] _segments;
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
                _segments = value;
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
            this.CopyProperties(newRoute, new string[] { "Segments"}, true);
            newRoute.Type = PathType.Route;
            return newRoute;
        }

        public DateTime StartTime
        {
            get
            {
                if(Type == PathType.Track && _segments.Length == 1)
                {
                    return _segments[0].StartTime;
                }
                return DateTime.MinValue;
            }
            set
            {
                if (Type == PathType.Track && _segments.Length == 1)
                {
                    _segments[0].StartTime = value;
                }
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (Type == PathType.Track && _segments.Length == 1)
                {
                    return _segments[0].EndTime;
                }
                return DateTime.MinValue;
            }
            set
            {
                if (Type == PathType.Track && _segments.Length == 1)
                {
                    _segments[0].EndTime= value;
                }
            }
        }

        /// <summary>
        /// Pas de eindtijd van een track aan, herbereken de tijden voor de tussenliggende waypoints
        /// Werkt alleen voor een track en als er maar 1 segment aanwezig is
        /// </summary>
        /// <param name="trackDuration">Duration of the track.</param>
        public void ResetDuration(TimeSpan trackDuration)
        {
            if (Type == PathType.Track && _segments.Length == 1)
            {
                EndTime = StartTime.Add(trackDuration);
                var timesChanger = new SegmentTimesRecalculator();
                var result = timesChanger.Recalculate(_segments[0], StartTime, trackDuration);
                _segments[0] = result;
            }
        }
    }
}