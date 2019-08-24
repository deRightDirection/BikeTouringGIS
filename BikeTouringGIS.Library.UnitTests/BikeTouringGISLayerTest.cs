using BikeTouringGIS.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using BikeTouringGISLibrary.Model;
using BikeTouringGISLibrary.Services;
using BikeTouringGISLibrary.GPX;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class BikeTouringGISLayerTest : UnitTestingBase
    {
        [TestMethod]
        [DeploymentItem("dwingeloo.gpx")]
        public void CreateLayer_Check_IsInEditMode_Is_False()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.IsInEditMode.Should().Be(false);
        }

        [TestMethod]
        [DeploymentItem("dwingeloo.gpx")]
        public void ChangeName_Check_IsInEditMode_Is_True()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.IsInEditMode.Should().Be(false);
            layer.Title = "Mannus";
            layer.IsInEditMode.Should().Be(true);
        }

        [TestMethod]
        [DeploymentItem("dwingeloo.gpx")]
        public void Title_Is_Correct()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.Title.Should().Be("MTB Dwingeloo");
        }

        [TestMethod]
        [DeploymentItem("63.gpx")]
        // bug #63
        public void Title_Is_Correct2()
        {
            var layer = CreateLayer("63.gpx");
            layer.Title.Should().Be("Holland Classic - 30 km");
        }

        [TestMethod]
        // bug #65
        public void Check_Extent_Is_Big_Enough_For_Also_Waypoints()
        {
            var layer = CreateLayer("65.gpx");
            layer.Extent.XMax.Should().BeGreaterOrEqualTo(6.732440000);
        }

        // bug #69
        [TestMethod]
        [DeploymentItem("65.gpx")]
        public void Save_And_Check_For_POIs()
        {
            string fileName = "65.gpx";
            string outFileName = "69.gpx";
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.WayPoints.Count.Should().Be(4);
            var wayPoints = new List<wptType>();
            gpxInfo.WayPoints.ForEach(x => wayPoints.Add(x));
            var layer = CreateLayer(fileName);
            layer.Save(wayPoints, Path.Combine(UnitTestDirectory, outFileName));
            path = Path.Combine(UnitTestDirectory, outFileName);
            gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.WayPoints.Count.Should().Be(4);
        }
        private BikeTouringGISLayer CreateLayer(string fileName)
        {
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.Tracks.ForEach(x => x.IsConvertedToRoute = true);
            var factory = new GeometryFactory(gpxInfo);
            factory.CreateGeometries();
            var layer = new BikeTouringGISLayer("testroute", gpxInfo.Routes.First());
            layer.SetExtentToFitWithWaypoints(gpxInfo.WayPointsExtent);
            return layer;
        }
    }
}