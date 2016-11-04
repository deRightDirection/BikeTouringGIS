using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using theRightDirection.Library.UnitTesting;
using BikeTouringGISLibrary;
using FluentAssertions;
using System.IO;

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
            var fileName = "Sample.gpx";
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = _fileReader.LoadFile(path);
            gpxInfo.Routes.Count.ShouldBeEquivalentTo(2);
            gpxInfo.Tracks.Count.ShouldBeEquivalentTo(11);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(49);
        }
    }
}
