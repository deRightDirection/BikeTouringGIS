using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using BikeTouringGISLibrary.GPX;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class GPXFileTest : UnitTestingBase
    {
        [TestMethod]
        // bug #70 (foute versie, komt door de gpxx extensions tag)
        public void Load_Incorrect_GPX_no_tracks()
        {
            var path = Path.Combine(UnitTestDirectory, "70.gpx");
            var gpxFile = new GPXFile(path);
            gpxFile.GetTracks().Count.Should().Be(0);
        }

        [TestMethod]
        [DeploymentItem("70_2.gpx")]
        // bug #70 (foute versie, maar de gpxx extensions tag verwijderd voor de track)
        public void Load_Incorrect_GPX_but_manually_fixed_one_track()
        {
            var path = Path.Combine(UnitTestDirectory, "70_2.gpx");
            var gpxFile = new GPXFile(path);
            gpxFile.GetTracks().Count.Should().Be(1);
        }
    }
}