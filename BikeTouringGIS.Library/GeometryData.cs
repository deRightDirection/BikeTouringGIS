﻿using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System.Collections.Generic;

namespace BikeTouringGISLibrary
{
    public abstract class GeometryData
    {
        public Envelope Extent { get; internal set; }
        public List<wptType> Points { get; set; }
        public string Name { get; internal set; }
        public BikeTouringGISGraphic Geometry { get; internal set; }
        public string FileName { get; set; }
    }
}