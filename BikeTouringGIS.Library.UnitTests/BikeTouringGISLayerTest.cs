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
using GPX;
using BikeTouringGISLibrary.Services;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class BikeTouringGISLayerTest : UnitTestingBase
    {
        [TestMethod]
        public void CreateLayer_Check_IsInEditMode_Is_False()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.IsInEditMode.ShouldBeEquivalentTo(false);
        }

        [TestMethod]
        public void ChangeName_Check_IsInEditMode_Is_True()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.IsInEditMode.ShouldBeEquivalentTo(false);
            layer.Title = "Mannus";
            layer.IsInEditMode.ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        // bug #63
        [DeploymentItem("dwingeloo.gpx")]
        public void Title_Is_Correct()
        {
            var layer = CreateLayer("dwingeloo.gpx");
            layer.Title.ShouldBeEquivalentTo("MTB Dwingeloo");
            layer = CreateLayer("63.gpx");
            layer.Title.ShouldBeEquivalentTo("Holland Classic - 30 km");
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
        public void Save_And_Check_For_POIs()
        {
            string fileName = "65.gpx";
            string outFileName = "69.gpx";
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(4);
            var wayPoints = new List<wptType>();
            gpxInfo.WayPoints.ForEach(x => wayPoints.Add(x));
            var layer = CreateLayer(fileName);
            layer.Save(wayPoints, Path.Combine(UnitTestDirectory, outFileName));
            path = Path.Combine(UnitTestDirectory, outFileName);
            gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(4);
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