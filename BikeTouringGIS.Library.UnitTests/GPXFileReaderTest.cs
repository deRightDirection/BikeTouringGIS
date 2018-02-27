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
        public void LoadFile_Counts_Tracks_Routes_Waypoints_Are_Correct()
        {
            var gpxInfo = LoadGPXData("Sample.gpx");
            gpxInfo.Routes.Count.ShouldBeEquivalentTo(2);
            gpxInfo.Tracks.Count.ShouldBeEquivalentTo(11);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(49);
        }

        [TestMethod]
        // bug #43
        public void LoadFile_Counts_Tracks_Routes_Waypoints_Are_Correct2()
        {
            var gpxInfo = LoadGPXData("BRM300BNK16 v2.gpx");
            gpxInfo.Routes.Count.ShouldBeEquivalentTo(0);
            gpxInfo.Tracks.Count.ShouldBeEquivalentTo(8);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(5);
        }

        [TestMethod]
        // #100 file gecorrigeerd en geen fouten
        public void LoadFile_Counts_Waypoints_Are_Correct()
        {
            var gpxInfo = LoadGPXData("pois.gpx");
            gpxInfo.Routes.Count.ShouldBeEquivalentTo(0);
            gpxInfo.Tracks.Count.ShouldBeEquivalentTo(0);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(157);
        }

        [TestMethod]
        // #100
        public void LoadFile_Counts_Waypoints_Are_Correct2()
        {
            var gpxInfo = LoadGPXData("pois2.gpx");
            gpxInfo.Routes.Count.ShouldBeEquivalentTo(0);
            gpxInfo.Tracks.Count.ShouldBeEquivalentTo(0);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(157);
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