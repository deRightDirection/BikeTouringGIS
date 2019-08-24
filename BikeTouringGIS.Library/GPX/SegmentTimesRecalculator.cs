using System;
using System.Collections.Generic;
using System.Linq;
namespace BikeTouringGISLibrary.GPX
{
    public class SegmentTimesRecalculator
    {
        private List<wptType> _waypoints;

        public trksegType Recalculate(trksegType segment, TimeSpan timeOffSet)
        {
            return Recalculate(segment, segment.StartTime, timeOffSet);
        }

        public trksegType Recalculate(trksegType segment, DateTime startTime, TimeSpan timeOffset)
        {
            _waypoints = segment.trkpt.ToList();
            var totalDistance = CalculateDistanceOfSegment(segment);
            var endTime = segment.StartTime.Add(timeOffset);
            var speed = totalDistance / timeOffset.TotalSeconds;
            _waypoints.Last().time = endTime;
            Recalculate(speed);
            segment.trkpt = _waypoints.ToArray();
            return segment;
        }

        private void Recalculate(double speedInMperS)
        {
            var dataAnalyzer = new DataAnalyzer(_waypoints);
            var distances = dataAnalyzer.Distances;
            var wayPointTime = _waypoints.First().time;
            for (int i = 0; i < _waypoints.Count - 2; i++)
            {
                var distance = distances[i];
                var time = distance / speedInMperS;
                var seconds = (int)Math.Truncate(time);
                var milliseconds = (int)((time - Math.Truncate(time)) * 1000);
                var timeOffset = new TimeSpan(0, 0, 0, seconds, milliseconds);
                _waypoints[i + 1].time = wayPointTime.Add(timeOffset);
                wayPointTime = _waypoints[i + 1].time;
            }
        }

        /// <summary>
        /// totale afstand in meters
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        private double CalculateDistanceOfSegment(trksegType segment)
        {
            var analyzer = new DataAnalyzer(segment.trkpt.ToList());
            var distance = analyzer.TotalDistance();
            return distance;
        }
    }
}