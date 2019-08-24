using Microsoft.VisualStudio.TestTools.UnitTesting;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using System.IO;
using BikeTouringGISLibrary.Model;
using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Services;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class GPXFileReaderTest : UnitTestingBase
    {
        private GeometryFactory _geometryFactory;

        [TestMethod]
        [DeploymentItem("dwingeloo.gpx")]
        // bug #69
        public void LoadFile_All_WayPoints_Do_Have_Description()
        {
            var gpxInfo = LoadGPXData("dwingeloo.gpx");
            foreach (var wpt in gpxInfo.WayPoints)
            {
                wpt.Name.Should().NotBeNullOrEmpty(wpt.Points[0].ToString());
            }
        }

        [TestMethod]
        [DeploymentItem("Sample.gpx")]
        // bug #34
        public void LoadFile_Check_If_All_Have_Length()
        {
            var gpxInfo = LoadGPXData("Sample.gpx");
            gpxInfo.Tracks.ForEach(x => x.IsConvertedToRoute = true);
            _geometryFactory.CreateGeometries();
            foreach (var route in gpxInfo.Routes)
            {
                var layer = new BikeTouringGISLayer("testroute", route);
                layer.Extent.Should().NotBeNull($"route, {route.Name}");
            }
        }

        [TestMethod]
        [DeploymentItem("Sample.gpx")]
        public void LoadFile_Counts_Tracks_Routes_Waypoints_Are_Correct()
        {
            var gpxInfo = LoadGPXData("Sample.gpx");
            gpxInfo.Routes.Count.Should().Be(2);
            gpxInfo.Tracks.Count.Should().Be(11);
            gpxInfo.WayPoints.Count.Should().Be(49);
        }

        [TestMethod]
        [DeploymentItem("BRM300BNK16 v2.gpx")]
        // bug #43
        public void LoadFile_Counts_Tracks_Routes_Waypoints_Are_Correct2()
        {
            var gpxInfo = LoadGPXData("BRM300BNK16 v2.gpx");
            gpxInfo.Routes.Count.Should().Be(0);
            gpxInfo.Tracks.Count.Should().Be(8);
            gpxInfo.WayPoints.Count.Should().Be(5);
        }

        [TestMethod]
        [DeploymentItem("pois.gpx")]
        // #100 file gecorrigeerd en geen fouten
        public void LoadFile_Counts_Waypoints_Are_Correct()
        {
            var gpxInfo = LoadGPXData("pois.gpx");
            gpxInfo.Routes.Count.Should().Be(0);
            gpxInfo.Routes.Count.Should().Be(0);
            gpxInfo.Tracks.Count.Should().Be(0);
            gpxInfo.WayPoints.Count.Should().Be(157);
        }

        [TestMethod]
        // #100
        [DeploymentItem("pois2.gpx")]
        public void LoadFile_Counts_Waypoints_Are_Correct2()
        {
            var gpxInfo = LoadGPXData("pois2.gpx");
            gpxInfo.Routes.Count.Should().Be(0);
            gpxInfo.Tracks.Count.Should().Be(0);
            gpxInfo.WayPoints.Count.Should().Be(157);
        }

        [TestInitialize]
        public void Setup()
        {
        }

        private GpxInformation LoadGPXData(string fileName)
        {
            var fileReader = new GpxFileReader();
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = fileReader.LoadFile(path);
            _geometryFactory = new GeometryFactory(gpxInfo);
            return gpxInfo;
        }
    }
}