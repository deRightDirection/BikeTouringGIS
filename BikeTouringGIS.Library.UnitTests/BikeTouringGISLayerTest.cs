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

        private BikeTouringGISLayer CreateLayer(string fileName)
        {
            var path = Path.Combine(UnitTestDirectory, fileName);
            var gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.Tracks.ForEach(x => x.ConvertTrackToRoute());
            gpxInfo.CreateGeometries();
            var layer = new BikeTouringGISLayer("testroute", gpxInfo.Routes.First());
            layer.SetExtentToFitWithWaypoints(gpxInfo.WayPointsExtent);
            return layer;
        }
    }
}