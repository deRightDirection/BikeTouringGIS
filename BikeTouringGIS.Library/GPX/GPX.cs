using System;
using System.Linq;
using System.Collections.Generic;

namespace BikeTouringGISLibrary.GPX
{
    public class Segment
    {
        public string Name { get; set; }

        public double Distance { get; set; }
        public double VerticalDistance { get; set; }
        public double FlatEarthDistance { get; set; }

        public double Time { get; set; }

        public double Velocity { get; set; }
        public double VerticalVelocity { get; set; }
        public double FlatEarthVelocity { get; set; }

        public double StartElevation { get; set; }
        public double EndElevation { get; set; }

        public double Course { get; set; }

        public double AbsoluteVerticalVelocity { get { return Math.Abs(VerticalVelocity); } }
        public double AbsoluteVerticalDistance { get { return Math.Abs(VerticalDistance); } }
    }

    public class Segue
    {
        public string Name { get; set; }

        public double Acceleration { get; set; }
        public double VerticalAcceleration { get; set; }
        public double FlatEarthAcceleration { get; set; }

        public double AbsoluteAcceleration { get { return Math.Abs(Acceleration); } }
        public double AbsoluteVerticalAcceleration { get { return Math.Abs(VerticalAcceleration); } }
        public double AbsoluteFlatEarthAcceleration { get { return Math.Abs(FlatEarthAcceleration); } }
    }
}