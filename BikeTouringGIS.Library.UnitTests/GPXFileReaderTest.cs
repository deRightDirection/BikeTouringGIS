using Microsoft.VisualStudio.TestTools.UnitTesting;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using System.IO;
using BikeTouringGISLibrary.Model;
using BikeTouringGIS.Controls;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class GPXFileReaderTest : UnitTestingBase
    {
        private GpxFileReader _fileReader;

        [TestInitialize]
        public void Setup()
        {
            _fileReader = new GpxFileReader();
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
        // bug #34
        public void LoadFile_Check_If_All_Have_Length()
        {
            var gpxInfo = LoadGPXData("Sample.gpx");
            gpxInfo.Tracks.ForEach(x => x.ConvertTrackToRoute());
            gpxInfo.CreateGeometries();
            foreach(var route in gpxInfo.Routes)
            {
                var layer = new BikeTouringGISLayer("testroute", route);
                layer.Extent.Should().NotBeNull($"route, {route.Name}");
            }
        }

        private GpxInformation LoadGPXData(string fileName)
        {
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = _fileReader.LoadFile(path);
            gpxInfo.CreateGeometries();
            return gpxInfo;
        }

    }
}
