using BikeTouringGIS.ViewModels;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Symbology;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using BikeTouringGISLibrary.Services;

namespace BikeTouringGISLibrary.UnitTests.ViewModels
{
    [TestClass]
    public class BikeTouringGISViewModelTest : UnitTestingBase
    {
        private BikeTouringGISViewModel _vm;
        private BikeTouringGISMapViewModel _map;

        [TestInitialize]
        public void Setup()
        {
            _vm = new BikeTouringGISViewModel();
            _map = new BikeTouringGISMapViewModel(true);
            _map.AddSymbol(GraphicType.GPXRoute, new SimpleLineSymbol());
            _map.AddSymbol(GraphicType.GPXRouteEndLocation, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.GPXRouteStartLocation, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.PoILabelL, new TextSymbol());
            _map.AddSymbol(GraphicType.PoILabelM, new TextSymbol());
            _map.AddSymbol(GraphicType.PoILabelXL, new TextSymbol());
            _map.AddSymbol(GraphicType.PointOfInterest, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.SplitPoint, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.SplitPointLabel, new TextSymbol());
            _map.AddSymbol(GraphicType.SplitRoute, new SimpleLineSymbol());
        }

        [TestMethod]
        // #85
        public void OpenGPXFile_Cant_Load_Two_Times_Same_File()
        {
            var filename = "85.gpx";
            var path = Path.Combine(UnitTestDirectory, filename);
            _vm.OpenGpxFile(_map, path);
            _map.BikeTouringGISLayers.Count.ShouldBeEquivalentTo(1);
            _vm.OpenGpxFile(_map, path);
            _map.BikeTouringGISLayers.Count.ShouldBeEquivalentTo(1);
        }

        [TestMethod]
        public void OpenGPXFile_Check_If_Extent_Is_Proper_Set()
        {
            var filename = "85_alleen_wpt.gpx";
            var path = Path.Combine(UnitTestDirectory, filename);
            var gpxFileInformation = new GpxFileReader().LoadFile(path);
            var geometryFactory = new GeometryFactory(gpxFileInformation);
            geometryFactory.CreateGeometries();
            var waypointsExtent = gpxFileInformation.WayPointsExtent.Expand(1.2);
            _vm.OpenGpxFile(_map, path);
            Assert.Fail("nog uitzoeken hoe extent te controleren is");
//            extent.XMax.ShouldBeEquivalentTo(waypointsExtent.XMax);
//            extent.XMin.ShouldBeEquivalentTo(waypointsExtent.XMin);
//            extent.YMax.ShouldBeEquivalentTo(waypointsExtent.YMax);
//            extent.YMin.ShouldBeEquivalentTo(waypointsExtent.YMin);
        }
    }
}